using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Data;
using BankingSystem.API.Interfaces;
using BankingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Repositories
{
    /// <summary>
    /// Provides Entity Framework Core data access for fraud detection flags.
    /// </summary>
    public class FraudFlagRepository : IFraudFlagRepository
    {
        private readonly AppDbContext _context;

        public FraudFlagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FraudFlag>> GetByTransactionIdAsync(Guid transactionId)
        {
            return await _context.FraudFlags
                .Where(f => f.TransactionId == transactionId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(FraudFlag fraudFlag)
        {
            await _context.FraudFlags.AddAsync(fraudFlag);
        }

        public async Task AddRangeAsync(IEnumerable<FraudFlag> fraudFlags)
        {
            await _context.FraudFlags.AddRangeAsync(fraudFlags);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}