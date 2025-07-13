using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;

namespace FraudSys.Domain.Services
{
    public class TransactionService(IClientRepository clientRepository) : ITransactionService
    {
        public async Task<object> ExecutePixTransaction(ExecutePixTransactionRequest request, CancellationToken cancellationToken)
        {
            request.Validate();
            var sourceClient = await clientRepository.GetClientByPkAsync(request.SourceClientDocument, cancellationToken)
                             ?? throw new InvalidOperationException("Cliente de origem não encontrado");

            ValidateClientDetails(request.SourceClientAgency, request.SourceClientAccount, sourceClient, "origem");

            var targetClient = await clientRepository.GetClientByPkAsync(request.TargetClientDocument, cancellationToken)
                ?? throw new InvalidOperationException("Cliente de destino não encontrado");

            ValidateClientDetails(request.TargetClientAgency, request.TargetClientAccount, targetClient, "destino");

            if (sourceClient.LimitePix < request.TransactionAmount)
            {
                throw new InvalidOperationException("Limite Pix insuficiente para a transação");
            }

            sourceClient.UpdatePixLimit(sourceClient.LimitePix - request.TransactionAmount);
            targetClient.UpdatePixLimit(targetClient.LimitePix + request.TransactionAmount);

            await clientRepository.UpdateClientsAsync([sourceClient, targetClient], cancellationToken);

            return new { ClientPixLimit = sourceClient.LimitePix };
        }

        private static void ValidateClientDetails(int requestAgency, string requestAccount, ClientEntity clientEntity, string clientType)
        {
            if (clientEntity.Agencia != requestAgency)
                throw new ArgumentException(string.Format("Agência do cliente de {0} não corresponde à agência solicitada", clientType));

            if (clientEntity.Conta != requestAccount)
                throw new ArgumentException(string.Format("Conta do cliente de {0} não corresponde à conta solicitada", clientType));
        }
    }
}
