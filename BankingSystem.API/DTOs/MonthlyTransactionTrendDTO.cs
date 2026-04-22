using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents monthly transaction count and volume for dashboard trend charts.
    /// </summary>
    public class MonthlyTransactionTrendDTO
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int TransactionCount { get; set; }

        public decimal TotalVolume { get; set; }
    }
}