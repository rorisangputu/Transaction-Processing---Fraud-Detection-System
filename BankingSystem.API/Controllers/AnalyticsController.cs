using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;
using BankingSystem.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    /// <summary>
    /// Exposes dashboard analytics endpoints for KPIs, trends, and risky accounts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<AnalyticsSummaryDTO>> GetSummary()
        {
            var summary = await _analyticsService.GetSummaryAsync();

            return Ok(summary);
        }

        [HttpGet("monthly-trends")]
        public async Task<ActionResult<IEnumerable<MonthlyTransactionTrendDTO>>> GetMonthlyTransactionTrends()
        {
            var trends = await _analyticsService.GetMonthlyTransactionTrendsAsync();

            return Ok(trends);
        }

        [HttpGet("risky-accounts")]
        public async Task<ActionResult<IEnumerable<RiskyAccountDTO>>> GetRiskyAccounts()
        {
            var riskyAccounts = await _analyticsService.GetRiskyAccountsAsync();

            return Ok(riskyAccounts);
        }
        [HttpGet("recent-flagged-transactions")]
        public async Task<ActionResult<IEnumerable<FlaggedTransactionDTO>>> GetRecentFlaggedTransactions()
        {
            var flaggedTransactions = await _analyticsService.GetRecentFlaggedTransactionsAsync();

            return Ok(flaggedTransactions);
        }

    }
}