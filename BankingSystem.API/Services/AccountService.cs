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
    /// Handles business logic for creating and retrieving bank accounts.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(
            IAccountRepository accountRepository,
            IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<AccountResponseDTO> CreateAccountAsync(CreateAccountDTO request)
        {
            if (request.UserId == Guid.Empty)
            {
                throw new ArgumentException("User id is required.");
            }

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                throw new ArgumentException("Account number is required.");
            }

            if (request.InitialBalance < 0)
            {
                throw new ArgumentException("Initial balance cannot be negative.");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                throw new KeyNotFoundException("User was not found.");
            }

            if (await _accountRepository.AccountNumberExistsAsync(request.AccountNumber))
            {
                throw new InvalidOperationException("An account with this account number already exists.");
            }

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                AccountNumber = request.AccountNumber.Trim(),
                Balance = request.InitialBalance,
                CreatedAt = DateTime.UtcNow
            };

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();

            return MapToResponseDTO(account);
        }

        public async Task<AccountResponseDTO?> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);

            return account is null ? null : MapToResponseDTO(account);
        }

        public async Task<AccountResponseDTO?> GetAccountByAccountNumberAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number is required.");
            }

            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber.Trim());

            return account is null ? null : MapToResponseDTO(account);
        }


        public async Task<IEnumerable<AccountResponseDTO>> GetAccountsByUserIdAsync(Guid userId)
        {
            var accounts = await _accountRepository.GetByUserIdAsync(userId);

            return accounts.Select(MapToResponseDTO);
        }

        private static AccountResponseDTO MapToResponseDTO(Account account)
        {
            return new AccountResponseDTO
            {
                Id = account.Id,
                UserId = account.UserId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                CreatedAt = account.CreatedAt
            };
        }
    }
}