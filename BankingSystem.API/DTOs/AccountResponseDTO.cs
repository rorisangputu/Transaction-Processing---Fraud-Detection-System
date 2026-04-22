using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents bank account data returned to API consumers.
    /// </summary>
    public class AccountResponseDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}