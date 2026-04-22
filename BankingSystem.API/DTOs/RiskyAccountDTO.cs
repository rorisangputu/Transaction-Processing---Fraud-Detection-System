using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents an account with elevated fraud activity for dashboard reporting.
    /// </summary>
    public class RiskyAccountDTO
    {
        public Guid AccountId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public int FraudFlagCount { get; set; }

        public decimal FlaggedTransactionVolume { get; set; }
    }
}