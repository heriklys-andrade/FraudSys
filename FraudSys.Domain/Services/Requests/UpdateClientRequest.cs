using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Domain.Services.Requests
{
    public class UpdateClientRequest
    {
        [FromRoute(Name = "clientDocument")]
        public required string ClientDocument { get; set; }
        [FromForm]
        public required int ClientAgency { get; set; }
        [FromForm]
        public required string ClientAccount { get; set; }
        [FromForm]
        public required double ClientPixLimit { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ClientDocument))
            {
                throw new ArgumentException("Documento do cliente deve ser preenchido", nameof(ClientDocument));
            }

            if (ClientAgency <= 0)
            {
                throw new ArgumentException("Agência do cliente deve deve maior que zero", nameof(ClientAgency));
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
