
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class AliCloudRemotePolicyModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}