namespace FraudSys.Domain.Services.Requests
{
    public class CreateClientRequest
    {
        public required string ClientDocument { get; set; }
        public required string ClientAgency { get; set; }
        public required string ClientAccount { get; set; }
        public required double ClientPixLimit { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ClientDocument))
            {
                throw new ArgumentException("Documento do cliente deve ser preenchido", nameof(ClientDocument));
            }

            if (string.IsNullOrWhiteSpace(ClientAgency))
            {
                throw new ArgumentException("Agência do cliente deve ser preenchida", nameof(ClientAgency));
            }

            if (string.IsNullOrWhiteSpace(ClientAccount))
            {
                throw new ArgumentException("Conta do cliente deve ser preenchida", nameof(ClientAccount));
            }

            if (ClientPixLimit <= 0)
            {
                throw new ArgumentException("Limite Pix do cliente deve ser maior que zero", nameof(ClientPixLimit));
            }
        }
    }
}
