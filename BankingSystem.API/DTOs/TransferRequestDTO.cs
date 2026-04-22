using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents the data required to transfer money between two accounts.
    /// </summary>
    public class TransferRequestDTO
    {
        public Guid SourceAccountId { get; set; }

        public Guid DestinationAccountId { get; set; }

        public decimal Amount { get; set; }
    }
}