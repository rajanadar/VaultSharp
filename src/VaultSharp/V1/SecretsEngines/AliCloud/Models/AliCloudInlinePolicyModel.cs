
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudInlinePolicyModel
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("policy_document")]
        public AliCloudInlinePolicyDocumentModel PolicyDocument { get; set; }
    }
}