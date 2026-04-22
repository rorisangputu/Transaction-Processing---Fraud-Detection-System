using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.DTOs
{
    /// <summary>
    /// Represents the data required to create a banking system user.
    /// </summary>
    public class CreateUserDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}