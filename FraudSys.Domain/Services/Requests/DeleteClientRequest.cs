using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Domain.Services.Requests
{
    public class DeleteClientRequest
    {
        [FromQuery]
        public required string ClientDocument { get; set; }
        [FromQuery]
        public required string ClientAgency { get; set; }
        [FromQuery]
        public required string ClientAccount { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ClientDocument))
            {
                throw new ArgumentException("Documento do cliente deve ser preenchido", nameof(ClientDocument));
            }

            if (ClientDocument.Length != 11)
            {
                throw new ArgumentException("Documento do cliente deve conter 11 caracteres", nameof(ClientDocument));
            }

            if (string.IsNullOrWhiteSpace(ClientAgency))
            {
                throw new ArgumentException("Agência do cliente deve ser preenchida", nameof(ClientAgency));
            }

            if (string.IsNullOrWhiteSpace(ClientAccount))
            {
                throw new ArgumentException("Conta do cliente deve ser preenchida", nameof(ClientAccount));
            }
        }
    }
}
