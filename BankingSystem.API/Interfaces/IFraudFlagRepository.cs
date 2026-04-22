using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines data access operations for fraud detection flags.
    /// </summary>
    public interface IFraudFlagRepository
    {
        Task<IEnumerable<FraudFlag>> GetByTransactionIdAsync(Guid transactionId);

        Task AddAsync(FraudFlag fraudFlag);

        Task AddRangeAsync(IEnumerable<FraudFlag> fraudFlags);

        Task SaveChangesAsync();
    }
}