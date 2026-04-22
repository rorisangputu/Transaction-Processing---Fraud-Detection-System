using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents the data required to create a bank account for a user.
    /// </summary>
    public class CreateAccountDTO
    {
        public Guid UserId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public decimal InitialBalance { get; set; }
    }
}