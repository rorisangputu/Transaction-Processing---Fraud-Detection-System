using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines analytics queries used by the dashboard.
    /// </summary>
    public interface IAnalyticsService
    {
        Task<AnalyticsSummaryDTO> GetSummaryAsync();

        Task<IEnumerable<MonthlyTransactionTrendDTO>> GetMonthlyTransactionTrendsAsync();

        Task<IEnumerable<RiskyAccountDTO>> GetRiskyAccountsAsync();
        Task<IEnumerable<FlaggedTransactionDTO>> GetRecentFlaggedTransactionsAsync();

    }
}