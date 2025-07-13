using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using FraudSys.Domain.Services.Responses;

namespace FraudSys.Domain.Services
{
    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        public async Task<GetClientResponse> GetClientAsync(GetClientRequest request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetClientByPkAsync(request.ClientId, cancellationToken) ?? throw new KeyNotFoundException("Cliente não encontrado");

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
    }
}
