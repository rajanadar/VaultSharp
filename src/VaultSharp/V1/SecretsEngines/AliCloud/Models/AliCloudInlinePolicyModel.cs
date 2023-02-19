
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudInlinePolicyModel
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("policy_document")]
        public AliCloudInlinePolicyDocumentModel PolicyDocument { get; set; }
    }
}