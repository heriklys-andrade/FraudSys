using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FraudSys.Domain.Services.Requests
{
    public class UpdateClientRequest
    {
        [FromRoute(Name = "clientDocument")]
        public required string ClientDocument { get; set; }
        [FromForm]
        public required string ClientAgency { get; set; }
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

            if (ClientPixLimit <= 0)
            {
                throw new ArgumentException("Limite Pix do cliente deve ser maior que zero", nameof(ClientPixLimit));
            }

            if (!Regex.IsMatch(ClientPixLimit.ToString(CultureInfo.InvariantCulture), @"^\d+(\.\d{1,2})?$"))
            {
                throw new ArgumentException("O valor do Limite Pix deve conter no máximo duas casas decimais", nameof(ClientPixLimit));
            }
        }
    }
}
