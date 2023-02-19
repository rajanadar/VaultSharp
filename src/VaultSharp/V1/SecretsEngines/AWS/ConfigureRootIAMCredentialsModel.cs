using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class ConfigureRootIAMCredentialsModel : RootIAMCredentialsConfigModel
    {
        [JsonPropertyName("secret_key")]
        public string SecretKey { get; set; }

        public ConfigureRootIAMCredentialsModel()
        {
            this.MaxRetries = -1;
        }
    }
}