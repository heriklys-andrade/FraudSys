using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;

namespace FraudSys.Domain.Services
{
    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        public async Task CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var existingClient = await clientRepository.GetClientByPkAsync(request.ClientDocument, cancellationToken);

            if (existingClient != null && existingClient.Documento == request.ClientDocument &&
                existingClient.Agencia == request.ClientAgency && existingClient.Conta == request.ClientAccount)
                throw new InvalidOperationException("Cliente já cadastrado");

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, request.ClientAccount, request.ClientPixLimit);

            await clientRepository.CreateClientAsync(client, cancellationToken);
        }

        public async Task<object> GetClientAsync(GetClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = await GetClientEntityAsync(request.ClientDocument, cancellationToken);

            VerifyClientDetails(request.ClientAgency, request.ClientAccount, client);

            return new { ClientPixLimit = client.LimitePix };
        }

        public async Task<object> UpdateClientAsync(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = await GetClientEntityAsync(request.ClientDocument, cancellationToken);

            VerifyClientDetails(request.ClientAgency, request.ClientAccount, client);

            client.UpdatePixLimit(request.ClientPixLimit);

            await clientRepository.UpdateClientAsync(client, cancellationToken);

            return new { ClientPixLimit = client.LimitePix };
        }

        public async Task DeleteClientAsync(DeleteClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = await GetClientEntityAsync(request.ClientDocument, cancellationToken);

            VerifyClientDetails(request.ClientAgency, request.ClientAccount, client);

            await clientRepository.DeleteClientAsync(client, cancellationToken);
        }

        private async Task<ClientEntity> GetClientEntityAsync(string clientDocument, CancellationToken cancellationToken)
        {
            return await clientRepository.GetClientByPkAsync(clientDocument, cancellationToken) ?? throw new KeyNotFoundException("Cliente não encontrado");
        }

        private static void VerifyClientDetails(int requestAgency, string requestAccount, ClientEntity client)
        {
            if (client.Agencia != requestAgency)
                throw new ArgumentException("Agência do cliente não corresponde à agência solicitada");

            if (client.Conta != requestAccount)
                throw new ArgumentException("Conta do cliente não corresponde à conta solicitada");
        }
    }
}