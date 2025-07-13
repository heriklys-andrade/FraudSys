using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Api.Controllers
{
    [Route("v1/[controller]")]
    public class ClientsController(IClientService clientService) : Controller
    {
        [HttpGet()]
        public async Task<IActionResult> GetClient(GetClientRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await clientService.GetClientAsync(request, cancellationToken));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
