using System.Diagnostics.CodeAnalysis;

namespace FraudSys.Domain.Environments
{
    [ExcludeFromCodeCoverage]
    public class AwsSettings
    {
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
