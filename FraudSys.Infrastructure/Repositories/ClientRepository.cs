using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Environments;
using FraudSys.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace FraudSys.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDynamoDBContext _context;

        public ClientRepository(IOptions<AwsSettings> awsSettings)
        {
            var credentials = new BasicAWSCredentials(awsSettings.Value.AccessKey, awsSettings.Value.SecretKey);

            var client = new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig() { RegionEndpoint = Amazon.RegionEndpoint.USEast2 });

            var ctxBuilder = new DynamoDBContextBuilder();

            _context = ctxBuilder
                .WithDynamoDBClient(() => client)
                .ConfigureContext(config => config.DisableFetchingTableMetadata = true)
                .Build();
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

        public async Task DeleteClientAsync(ClientEntity client, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(client, cancellationToken);
        }
    }
}
