using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class CreateRootCredentialsConfigModel : RootCredentialsConfigModel
    {
        [JsonPropertyName("secret_key")]
        public string SecretKey { get; set; }
    }
}