
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudInlinePolicyDocumentStatementModel
    {
        [JsonPropertyName("Action")]
        public string Action { get; set; }

        [JsonPropertyName("Effect")]
        public string Effect { get; set; }

        [JsonPropertyName("Resource")]
        public string Resource { get; set; }
    }
}