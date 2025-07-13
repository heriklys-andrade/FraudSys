using FraudSys.Domain.Services.Requests;
using FraudSys.Domain.Services.Responses;

namespace FraudSys.Domain.Interfaces.Services
{
    public interface IClientService
    {
        Task CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
        Task<GetClientResponse> GetClientAsync(GetClientRequest request, CancellationToken cancellationToken);
        Task<GetClientResponse> UpdateClientAsync(UpdateClientRequest request, CancellationToken cancellationToken);
    }
}
