using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines data access operations for banking system users.
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<IEnumerable<User>> GetAllAsync();

        Task AddAsync(User user);

        Task<bool> EmailExistsAsync(string email);

        Task SaveChangesAsync();
    }
}