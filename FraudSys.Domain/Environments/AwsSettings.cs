using System.Diagnostics.CodeAnalysis;

namespace FraudSys.Domain.Environments
{
    [ExcludeFromCodeCoverage]
    public class AwsSettings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
