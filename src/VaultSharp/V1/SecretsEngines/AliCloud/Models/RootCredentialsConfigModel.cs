using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class RootCredentialsConfigModel
    {
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }
    }
}