using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Api.Controllers
{
    [Route("v1/[controller]")]
    public class ClientsController(IClientService clientService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            await clientService.CreateClientAsync(request, cancellationToken);
            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(GetClientRequest request, CancellationToken cancellationToken)
        {
            return Ok(await clientService.GetClientAsync(request, cancellationToken));
        }

        [HttpPut("{clientDocument}")]
        public async Task<IActionResult> UpdateClient(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            return Ok(await clientService.UpdateClientAsync(request, cancellationToken));
        }
    }
}
