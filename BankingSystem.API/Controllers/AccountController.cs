using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;
using BankingSystem.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for creating and retrieving bank accounts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult<AccountResponseDTO>> CreateAccount(CreateAccountDTO request)
        {
            try
            {
                var account = await _accountService.CreateAccountAsync(request);
                return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountResponseDTO>> GetAccountById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);

            if (account is null)
            {
                return NotFound("Account was not found.");
            }

            return Ok(account);
        }
        [HttpGet("number/{accountNumber}")]
        public async Task<ActionResult<AccountResponseDTO>> GetAccountByAccountNumber(string accountNumber)
        {
            try
            {
                var account = await _accountService.GetAccountByAccountNumberAsync(accountNumber);

                if (account is null)
                {
                    return NotFound("Account was not found.");
                }

                return Ok(account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<AccountResponseDTO>>> GetAccountsByUserId(Guid userId)
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(userId);

            return Ok(accounts);
        }
    }
}