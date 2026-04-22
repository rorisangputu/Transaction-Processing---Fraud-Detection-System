using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents user data returned to API consumers.
    /// </summary>
    public class UserResponseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}