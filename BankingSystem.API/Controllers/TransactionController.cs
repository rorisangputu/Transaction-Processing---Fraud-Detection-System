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
    /// Exposes endpoints for processing and viewing financial transactions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<TransactionResponseDTO>> Deposit(DepositRequestDTO request)
        {
            try
            {
                var transaction = await _transactionService.DepositAsync(request);
                return Ok(transaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<TransactionResponseDTO>> Withdraw(WithdrawRequestDTO request)
        {
            try
            {
                var transaction = await _transactionService.WithdrawAsync(request);
                return Ok(transaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransactionResponseDTO>> Transfer(TransferRequestDTO request)
        {
            try
            {
                var transaction = await _transactionService.TransferAsync(request);
                return Ok(transaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("account/{accountId:guid}")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetByAccountId(Guid accountId)
        {
            var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(transactions);
        }
    }
}