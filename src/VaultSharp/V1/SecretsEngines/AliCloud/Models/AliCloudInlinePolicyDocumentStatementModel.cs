
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudInlinePolicyDocumentStatementModel
    {
        [JsonProperty("Action")]
        public string Action { get; set; }

        [JsonProperty("Effect")]
        public string Effect { get; set; }

        [JsonProperty("Resource")]
        public string Resource { get; set; }
    }
}