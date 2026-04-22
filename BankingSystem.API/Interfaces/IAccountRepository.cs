using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;

namespace BankingSystem.API.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id);

        Task<Account?> GetByAccountNumberAsync(string accountNumber);

        Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId);

        Task AddAsync(Account account);

        void Update(Account account);

        Task<bool> AccountNumberExistsAsync(string accountNumber);

        Task SaveChangesAsync();
    }
}