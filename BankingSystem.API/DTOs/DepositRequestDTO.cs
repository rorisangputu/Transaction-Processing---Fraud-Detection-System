using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents the data required to deposit money into an account.
    /// </summary>
    public class DepositRequestDTO
    {
        public Guid DestinationAccountId { get; set; }

        public decimal Amount { get; set; }
    }
}