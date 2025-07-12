using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Domain.Entities
{
    [DynamoDBTable("clients")]
    public class ClientEntity
    {
        public ClientEntity()
        {

        }

        public ClientEntity(string documento, int agencia, string conta, double limitePix)
        {
            Documento = documento;
            Agencia = agencia;
            Conta = conta;
            LimitePix = limitePix;
        }

        [DynamoDBHashKey("document")]
        public string Documento { get; private set; }

        [DynamoDBProperty("agency")]
        public int Agencia { get; private set; }

        [DynamoDBProperty("account")]
        public string Conta { get; private set; }

        [DynamoDBProperty("pix_limit")]
        public double LimitePix { get; private set; }
    }
}