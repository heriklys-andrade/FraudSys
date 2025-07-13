using FraudSys.Domain.Entities;

namespace FraudSys.Domain.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task CreateClientAsync(ClientEntity client, CancellationToken cancellationToken);
        Task DeleteClientAsync(string pk);
        Task<ClientEntity> GetClientByPkAsync(string pk, CancellationToken cancellationToken);
        Task UpdateClientAsync(ClientEntity client);
    }
}
