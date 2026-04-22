using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;
using BankingSystem.API.Interfaces;
using BankingSystem.API.Models;

namespace BankingSystem.API.Services
{
    /// <summary>
    /// Handles business logic for deposits, withdrawals, transfers, and transaction history.
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFraudDetectionService _fraudDetectionService;
        private readonly IFraudFlagRepository _fraudFlagRepository;


        public TransactionService(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IFraudDetectionService fraudDetectionService,
            IFraudFlagRepository fraudFlagRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _fraudDetectionService = fraudDetectionService;
            _fraudFlagRepository = fraudFlagRepository;
        }


        public async Task<TransactionResponseDTO> DepositAsync(DepositRequestDTO request)
        {
            ValidateAmount(request.Amount);

            var destinationAccount = await _accountRepository.GetByIdAsync(request.DestinationAccountId);

            if (destinationAccount is null)
            {
                throw new KeyNotFoundException("Destination account was not found.");
            }

            destinationAccount.Balance += request.Amount;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Type = TransactionType.Deposit,
                Amount = request.Amount,
                DestinationAccountId = destinationAccount.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            var fraudFlags = (await _fraudDetectionService.EvaluateTransactionAsync(transaction)).ToList();

            if (fraudFlags.Any())
            {
                await _fraudFlagRepository.AddRangeAsync(fraudFlags);
                await _fraudFlagRepository.SaveChangesAsync();

                transaction.FraudFlags = fraudFlags;
            }

            return MapToResponseDTO(transaction);
        }

        public async Task<TransactionResponseDTO> WithdrawAsync(WithdrawRequestDTO request)
        {
            ValidateAmount(request.Amount);

            var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId);

            if (sourceAccount is null)
            {
                throw new KeyNotFoundException("Source account was not found.");
            }

            if (sourceAccount.Balance < request.Amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            sourceAccount.Balance -= request.Amount;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Type = TransactionType.Withdraw,
                Amount = request.Amount,
                SourceAccountId = sourceAccount.Id,
                CreatedAt = DateTime.UtcNow
            };

            _accountRepository.Update(sourceAccount);
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            var fraudFlags = (await _fraudDetectionService.EvaluateTransactionAsync(transaction)).ToList();

            if (fraudFlags.Any())
            {
                await _fraudFlagRepository.AddRangeAsync(fraudFlags);
                await _fraudFlagRepository.SaveChangesAsync();

                transaction.FraudFlags = fraudFlags;
            }

            return MapToResponseDTO(transaction);
        }

        public async Task<TransactionResponseDTO> TransferAsync(TransferRequestDTO request)
        {
            ValidateAmount(request.Amount);

            if (request.SourceAccountId == request.DestinationAccountId)
            {
                throw new InvalidOperationException("Source and destination accounts cannot be the same.");
            }

            var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId);
            var destinationAccount = await _accountRepository.GetByIdAsync(request.DestinationAccountId);

            if (sourceAccount is null)
            {
                throw new KeyNotFoundException("Source account was not found.");
            }

            if (destinationAccount is null)
            {
                throw new KeyNotFoundException("Destination account was not found.");
            }

            if (sourceAccount.Balance < request.Amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            sourceAccount.Balance -= request.Amount;
            destinationAccount.Balance += request.Amount;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Type = TransactionType.Transfer,
                Amount = request.Amount,
                SourceAccountId = sourceAccount.Id,
                DestinationAccountId = destinationAccount.Id,
                CreatedAt = DateTime.UtcNow
            };

            _accountRepository.Update(sourceAccount);
            _accountRepository.Update(destinationAccount);
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            var fraudFlags = (await _fraudDetectionService.EvaluateTransactionAsync(transaction)).ToList();

            if (fraudFlags.Any())
            {
                await _fraudFlagRepository.AddRangeAsync(fraudFlags);
                await _fraudFlagRepository.SaveChangesAsync();

                transaction.FraudFlags = fraudFlags;
            }

            return MapToResponseDTO(transaction);
        }

        public async Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);

            return transactions.Select(MapToResponseDTO);
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Transaction amount must be greater than zero.");
            }
        }

        private static TransactionResponseDTO MapToResponseDTO(Transaction transaction)
        {
            return new TransactionResponseDTO
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Amount = transaction.Amount,
                SourceAccountId = transaction.SourceAccountId,
                DestinationAccountId = transaction.DestinationAccountId,
                CreatedAt = transaction.CreatedAt,
                IsFlagged = transaction.FraudFlags.Any(),
                FraudReasons = transaction.FraudFlags.Select(flag => flag.Reason).ToList()
            };
        }
    }
}