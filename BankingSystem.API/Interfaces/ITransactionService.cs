using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines business operations for processing financial transactions.
    /// </summary>
    public interface ITransactionService
    {
        Task<TransactionResponseDTO> DepositAsync(DepositRequestDTO request);

        Task<TransactionResponseDTO> WithdrawAsync(WithdrawRequestDTO request);

        Task<TransactionResponseDTO> TransferAsync(TransferRequestDTO request);

        Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByAccountIdAsync(Guid accountId);
    }
}