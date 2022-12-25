using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class CreateRootCredentialsConfigModel : RootCredentialsConfigModel
    {
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }
    }
}