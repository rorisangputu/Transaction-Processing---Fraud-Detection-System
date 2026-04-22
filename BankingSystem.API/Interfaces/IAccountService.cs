using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;

namespace BankingSystem.API.Interfaces
{
    /// <summary>
    /// Defines business operations for creating and retrieving bank accounts.
    /// </summary>
    public interface IAccountService
    {
        Task<AccountResponseDTO> CreateAccountAsync(CreateAccountDTO request);

        Task<AccountResponseDTO?> GetAccountByIdAsync(Guid id);
        Task<AccountResponseDTO?> GetAccountByAccountNumberAsync(string accountNumber);


        Task<IEnumerable<AccountResponseDTO>> GetAccountsByUserIdAsync(Guid userId);
    }
}