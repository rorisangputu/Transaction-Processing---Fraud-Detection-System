using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents high-level analytics metrics for the dashboard.
    /// </summary>
    public class AnalyticsSummaryDTO
    {
        public int TotalTransactions { get; set; }

        public decimal TotalVolume { get; set; }

        public int FraudFlagCount { get; set; }

        public decimal FraudRate { get; set; }
    }
}