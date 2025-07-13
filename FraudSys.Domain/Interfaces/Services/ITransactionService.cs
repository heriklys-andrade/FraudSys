using FraudSys.Domain.Services.Requests;
using FraudSys.Domain.Services.Responses;

namespace FraudSys.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<GetClientResponse> ExecutePixTransaction(ExecutePixTransactionRequest request, CancellationToken cancellationToken);
    }
}
