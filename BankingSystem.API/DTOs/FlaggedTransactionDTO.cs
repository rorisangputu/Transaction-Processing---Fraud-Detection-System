using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents a recently flagged transaction for dashboard review.
    /// </summary>
    public class FlaggedTransactionDTO
    {
        public Guid TransactionId { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public Guid? SourceAccountId { get; set; }

        public Guid? DestinationAccountId { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<string> FraudReasons { get; set; } = new List<string>();
    }
}