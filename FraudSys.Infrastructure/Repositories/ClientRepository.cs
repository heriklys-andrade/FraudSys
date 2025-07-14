using Amazon.DynamoDBv2.DataModel;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace FraudSys.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ClientRepository : IClientRepository
    {
        private readonly IDynamoDBContext _context;

        public ClientRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task CreateClientAsync(ClientEntity client, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(client, cancellationToken);
        }

        public async Task<ClientEntity> GetClientByPkAsync(string pk, CancellationToken cancellationToken)
        {
            return await _context.LoadAsync<ClientEntity>(pk, cancellationToken);
        }

        public async Task UpdateClientAsync(ClientEntity client, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(client, cancellationToken);
        }

        public async Task UpdateClientsAsync(IEnumerable<ClientEntity> clients, CancellationToken cancellationToken)
        {
            var batch = _context.CreateBatchWrite<ClientEntity>();

            batch.AddPutItems(clients);

            await batch.ExecuteAsync(cancellationToken);
        }

        public async Task DeleteClientAsync(ClientEntity client, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(client, cancellationToken);
        }
    }
}
