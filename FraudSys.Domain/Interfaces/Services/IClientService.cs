using FraudSys.Domain.Services.Requests;

namespace FraudSys.Domain.Interfaces.Services
{
    public interface IClientService
    {
        Task CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
        Task<object> GetClientAsync(GetClientRequest request, CancellationToken cancellationToken);
        Task<object> UpdateClientAsync(UpdateClientRequest request, CancellationToken cancellationToken);
        Task DeleteClientAsync(DeleteClientRequest request, CancellationToken cancellationToken);
    }
}
