using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using FraudSys.Domain.Services.Responses;

namespace FraudSys.Domain.Services
{
    public class TransactionService(IClientRepository clientRepository) : ITransactionService
    {
        public async Task<GetClientResponse> ExecutePixTransaction(ExecutePixTransactionRequest request, CancellationToken cancellationToken)
        {
            request.Validate();

            var sourceClient = await clientRepository.GetClientByPkAsync(request.SourceClientDocument, cancellationToken)
                ?? throw new KeyNotFoundException("Cliente de origem não encontrado");

            if (sourceClient.Agencia != request.SourceClientAgency)
            {
                throw new ArgumentException("Agência do cliente de origem não corresponde à agência solicitada");
            }

            if (sourceClient.Conta != request.SourceClientAccount)
            {
                throw new ArgumentException("Conta do cliente de origem não corresponde à conta solicitada");
            }

            var targetClient = await clientRepository.GetClientByPkAsync(request.TargetClientDocument, cancellationToken)
                ?? throw new KeyNotFoundException("Cliente de destino não encontrado");

            if (targetClient.Agencia != request.TargetClientAgency)
            {
                throw new ArgumentException("Agência do cliente de destino não corresponde à agência solicitada");
            }

            if (targetClient.Conta != request.TargetClientAccount)
            {
                throw new ArgumentException("Conta do cliente de destino não corresponde à conta solicitada");
            }

            if (sourceClient.LimitePix < request.TransactionAmount)
            {
                throw new InvalidOperationException("Limite Pix insuficiente para a transação");
            }

            sourceClient.UpdatePixLimit(sourceClient.LimitePix - request.TransactionAmount);
            targetClient.UpdatePixLimit(targetClient.LimitePix + request.TransactionAmount);

            await clientRepository.UpdateClientsAsync([sourceClient, targetClient], cancellationToken);

            return new GetClientResponse
            {
                ClientPixLimit = sourceClient.LimitePix
            };
        }
    }
}
