using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using FraudSys.Domain.Services.Responses;

namespace FraudSys.Domain.Services
{
    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        public async Task CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, request.ClientAccount, request.ClientPixLimit);

            await clientRepository.CreateClientAsync(client, cancellationToken);
        }

        public async Task<GetClientResponse> GetClientAsync(GetClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = await clientRepository.GetClientByPkAsync(request.ClientDocument, cancellationToken) ?? throw new KeyNotFoundException("Cliente não encontrado");

            if (client.Agencia != request.ClientAgency)
            {
                throw new ArgumentException("Agência do cliente não corresponde à agência solicitada");
            }

            if (client.Conta != request.ClientAccount)
            {
                throw new ArgumentException("Conta do cliente não corresponde à conta solicitada");
            }

            return new GetClientResponse
            {
                ClientPixLimit = client.LimitePix
            };
        }

        public async Task<GetClientResponse> UpdateClientAsync(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = await clientRepository.GetClientByPkAsync(request.ClientDocument, cancellationToken) ?? throw new KeyNotFoundException("Cliente não encontrado");

            if (client.Agencia != request.ClientAgency)
            {
                throw new ArgumentException("Agência do cliente não corresponde à agência solicitada");
            }

            if (client.Conta != request.ClientAccount)
            {
                throw new ArgumentException("Conta do cliente não corresponde à conta solicitada");
            }

            client.UpdatePixLimit(request.ClientPixLimit);

            await clientRepository.UpdateClientAsync(client, cancellationToken);

            return new GetClientResponse
            {
                ClientPixLimit = client.LimitePix
            };
        }

        public async Task DeleteClientAsync(GetClientRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var client = await clientRepository.GetClientByPkAsync(request.ClientDocument, cancellationToken) ?? throw new KeyNotFoundException("Cliente não encontrado");

            if (client.Agencia != request.ClientAgency)
            {
                throw new ArgumentException("Agência do cliente não corresponde à agência solicitada");
            }

            if (client.Conta != request.ClientAccount)
            {
                throw new ArgumentException("Conta do cliente não corresponde à conta solicitada");
            }

            await clientRepository.DeleteClientAsync(client, cancellationToken);
        }
    }
}