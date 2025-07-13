using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace FraudSys.Domain.Services.Requests
{
    public class GetClientRequest
    {
        [FromQuery]
        public required string ClientId { get; set; }
        [FromQuery]
        public required int ClientAgency { get; set; }
        [FromQuery]
        public required string ClientAccount { get; set; }

        [JsonIgnore]
        public bool IsValid { get; set; }

        [JsonIgnore]
        public ICollection<string> ErrorMessages { get; set; }

        public void Validate()
        {
            ErrorMessages = new List<string>();



        }
    }
}
