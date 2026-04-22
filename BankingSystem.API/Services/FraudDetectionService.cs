using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Interfaces;
using BankingSystem.API.Models;

namespace BankingSystem.API.Services
{
    /// <summary>
    /// Applies rule-based fraud detection checks to completed financial transactions.
    /// </summary>
    public class FraudDetectionService : IFraudDetectionService
    {
        private const decimal HighValueThreshold = 100_000m;
        private const int RapidTransactionCountThreshold = 3;
        private static readonly TimeSpan RapidTransactionWindow = TimeSpan.FromMinutes(5);

        private readonly ITransactionRepository _transactionRepository;

        public FraudDetectionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<FraudFlag>> EvaluateTransactionAsync(Transaction transaction)
        {
            var fraudFlags = new List<FraudFlag>();

            if (transaction.Amount >= HighValueThreshold)
            {
                fraudFlags.Add(CreateFlag(transaction.Id, "High-value transaction detected."));
            }

            var accountId = transaction.SourceAccountId ?? transaction.DestinationAccountId;

            if (accountId.HasValue)
            {
                var recentTransactions = await _transactionRepository.GetRecentByAccountIdAsync(accountId.Value, RapidTransactionCountThreshold + 1);

                var rapidTransactions = recentTransactions
                    .Where(t => t.Id != transaction.Id)
                    .Where(t => transaction.CreatedAt - t.CreatedAt <= RapidTransactionWindow)
                    .ToList();

                if (rapidTransactions.Count >= RapidTransactionCountThreshold)
                {
                    fraudFlags.Add(CreateFlag(transaction.Id, "Multiple transactions detected within a short time window."));
                }
            }

            if (transaction.Type == TransactionType.Transfer &&
                transaction.SourceAccountId == transaction.DestinationAccountId)
            {
                fraudFlags.Add(CreateFlag(transaction.Id, "Transfer source and destination accounts are the same."));
            }

            return fraudFlags;
        }

        private static FraudFlag CreateFlag(Guid transactionId, string reason)
        {
            return new FraudFlag
            {
                Id = Guid.NewGuid(),
                TransactionId = transactionId,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}