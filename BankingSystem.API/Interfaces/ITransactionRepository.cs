using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines data access operations for financial transactions.
    /// </summary>
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);

        Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);

        Task<IEnumerable<Transaction>> GetRecentByAccountIdAsync(Guid accountId, int count);

        Task AddAsync(Transaction transaction);

        Task SaveChangesAsync();
    }
}