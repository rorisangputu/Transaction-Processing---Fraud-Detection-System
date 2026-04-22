using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines business operations for banking system users.
    /// </summary>
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(CreateUserDTO request);

        Task<UserResponseDTO?> GetUserByIdAsync(Guid id);

        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    }
}