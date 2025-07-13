using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace FraudSys.Domain.Services.Requests
{
    public class GetClientRequest
    {
        [FromQuery]
        public required string ClientDocument { get; set; }
        [FromQuery]
        public required int ClientAgency { get; set; }
        [FromQuery]
        public required string ClientAccount { get; set; }

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
        }
    }
}
