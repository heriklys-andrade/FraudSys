using FraudSys.Domain.Services.Requests;
using FraudSys.Domain.Services.Responses;

namespace FraudSys.Domain.Interfaces.Services
{
    public interface IClientService
    {
        Task<GetClientResponse> GetClientAsync(GetClientRequest request, CancellationToken cancellationToken);
    }
}
