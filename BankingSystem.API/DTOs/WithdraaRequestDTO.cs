using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents the data required to withdraw money from an account.
    /// </summary>
    public class WithdrawRequestDTO
    {
        public Guid SourceAccountId { get; set; }

        public decimal Amount { get; set; }
    }
}