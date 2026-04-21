using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.API.Models
{
    /// <summary>
    /// Defines the supported transaction types in the banking system.
    /// </summary>
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
    }
}