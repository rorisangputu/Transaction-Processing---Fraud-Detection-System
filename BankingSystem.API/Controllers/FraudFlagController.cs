using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.Interfaces;
using BankingSystem.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for viewing fraud detection flags.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FraudFlagController : ControllerBase
    {
        private readonly IFraudFlagRepository _fraudFlagRepository;

        public FraudFlagController(IFraudFlagRepository fraudFlagRepository)
        {
            _fraudFlagRepository = fraudFlagRepository;
        }

        [HttpGet("transaction/{transactionId:guid}")]
        public async Task<ActionResult<IEnumerable<FraudFlag>>> GetByTransactionId(Guid transactionId)
        {
            var fraudFlags = await _fraudFlagRepository.GetByTransactionIdAsync(transactionId);

            return Ok(fraudFlags);
        }
    }
}