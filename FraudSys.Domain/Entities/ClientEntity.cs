using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Domain.Entities
{
    [DynamoDBTable("clients")]
    public class ClientEntity
    {
        public ClientEntity()
        {

        }

        public ClientEntity(string documento, string agencia, string conta, double limitePix)
        {
            Documento = documento;
            Agencia = agencia;
            Conta = conta;
            LimitePix = limitePix;
        }

        [DynamoDBHashKey("document")]
        public string Documento { get; private set; }

        [DynamoDBProperty("agency")]
        public string Agencia { get; private set; }

        [DynamoDBProperty("account")]
        public string Conta { get; private set; }

        [DynamoDBProperty("pix_limit")]
        public double LimitePix { get; private set; }

        public void UpdatePixLimit(double novoLimite)
        {
            if (novoLimite <= 0)
            {
                throw new ArgumentException("Limite Pix deve ser maior que zero", nameof(novoLimite));
            }

            LimitePix = novoLimite;
        }
    }
}