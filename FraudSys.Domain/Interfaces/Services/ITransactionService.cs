using FraudSys.Domain.Services.Requests;

namespace FraudSys.Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<object> ExecutePixTransaction(ExecutePixTransactionRequest request, CancellationToken cancellationToken);
    }
}
