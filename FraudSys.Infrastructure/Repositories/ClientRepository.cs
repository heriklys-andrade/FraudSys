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

        public async Task<ClientEntity> GetClientByPkAsync(string pk, CancellationToken cancellationToken)
        {
            return await _context.LoadAsync<ClientEntity>(pk, cancellationToken);
        }

        public Task AddClientAsync(ClientEntity client)
        {
            throw new NotImplementedException();
        }

        public Task UpdateClientAsync(ClientEntity client)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClientAsync(string pk)
        {
            throw new NotImplementedException();
        }
    }
}
