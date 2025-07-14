using FraudSys.Domain.Services.Requests;
using Microsoft.AspNetCore.Mvc;
using FraudSys.Domain.Interfaces.Services;

namespace FraudSys.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TransactionsController(ITransactionService transactionService) : Controller
    {
        [HttpPost("pix")]
        public async Task<IActionResult> ExecuteTransaction([FromBody] ExecutePixTransactionRequest request, CancellationToken cancellationToken)
        {
            return Ok(await transactionService.ExecutePixTransaction(request, cancellationToken));
        }
    }
}
