using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Models;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines rule-based fraud detection for financial transactions.
    /// </summary>
    public interface IFraudDetectionService
    {
        Task<IEnumerable<FraudFlag>> EvaluateTransactionAsync(Transaction transaction);
    }
}