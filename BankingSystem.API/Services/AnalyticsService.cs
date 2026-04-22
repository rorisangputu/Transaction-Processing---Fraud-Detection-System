using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Data;
using BankingSystem.API.DTOs;
using BankingSystem.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Services
{
    /// <summary>
    /// Handles analytics queries for transaction volume, fraud rate, trends, and risky accounts.
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        private readonly AppDbContext _context;

        public AnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsSummaryDTO> GetSummaryAsync()
        {
            var totalTransactions = await _context.Transactions.CountAsync();
            var totalVolume = await _context.Transactions.SumAsync(t => t.Amount);
            var fraudFlagCount = await _context.FraudFlags.CountAsync();

            var fraudRate = totalTransactions == 0
                ? 0
                : Math.Round((decimal)fraudFlagCount / totalTransactions * 100, 2);

            return new AnalyticsSummaryDTO
            {
                TotalTransactions = totalTransactions,
                TotalVolume = totalVolume,
                FraudFlagCount = fraudFlagCount,
                FraudRate = fraudRate
            };
        }

        public async Task<IEnumerable<MonthlyTransactionTrendDTO>> GetMonthlyTransactionTrendsAsync()
        {
            return await _context.Transactions
                .GroupBy(t => new { t.CreatedAt.Year, t.CreatedAt.Month })
                .Select(group => new MonthlyTransactionTrendDTO
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    TransactionCount = group.Count(),
                    TotalVolume = group.Sum(t => t.Amount)
                })
                .OrderBy(trend => trend.Year)
                .ThenBy(trend => trend.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<RiskyAccountDTO>> GetRiskyAccountsAsync()
        {
            var flaggedAccountStats = _context.FraudFlags
                .Where(flag =>
                    flag.Transaction.SourceAccountId.HasValue ||
                    flag.Transaction.DestinationAccountId.HasValue)
                .Select(flag => new
                {
                    AccountId = flag.Transaction.SourceAccountId.HasValue
                        ? flag.Transaction.SourceAccountId.Value
                        : flag.Transaction.DestinationAccountId!.Value,
                    TransactionAmount = flag.Transaction.Amount
                })
                .GroupBy(item => item.AccountId)
                .Select(group => new
                {
                    AccountId = group.Key,
                    FraudFlagCount = group.Count(),
                    FlaggedTransactionVolume = group.Sum(item => item.TransactionAmount)
                });

            return await flaggedAccountStats
                .Join(
                    _context.Accounts,
                    flaggedAccount => flaggedAccount.AccountId,
                    account => account.Id,
                    (flaggedAccount, account) => new RiskyAccountDTO
                    {
                        AccountId = account.Id,
                        AccountNumber = account.AccountNumber,
                        FraudFlagCount = flaggedAccount.FraudFlagCount,
                        FlaggedTransactionVolume = flaggedAccount.FlaggedTransactionVolume
                    })
                .OrderByDescending(account => account.FraudFlagCount)
                .ThenByDescending(account => account.FlaggedTransactionVolume)
                .Take(10)
                .ToListAsync();
        }


        public async Task<IEnumerable<FlaggedTransactionDTO>> GetRecentFlaggedTransactionsAsync()
        {
            return await _context.Transactions
                .Include(transaction => transaction.FraudFlags)
                .Where(transaction => transaction.FraudFlags.Any())
                .OrderByDescending(transaction => transaction.CreatedAt)
                .Take(10)
                .Select(transaction => new FlaggedTransactionDTO
                {
                    TransactionId = transaction.Id,
                    Type = transaction.Type,
                    Amount = transaction.Amount,
                    SourceAccountId = transaction.SourceAccountId,
                    DestinationAccountId = transaction.DestinationAccountId,
                    CreatedAt = transaction.CreatedAt,
                    FraudReasons = transaction.FraudFlags
                        .Select(flag => flag.Reason)
                        .ToList()
                })
                .ToListAsync();
        }

    }
}