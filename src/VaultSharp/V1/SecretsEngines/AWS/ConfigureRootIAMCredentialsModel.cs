using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class ConfigureRootIAMCredentialsModel : RootIAMCredentialsConfigModel
    {
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }

        public ConfigureRootIAMCredentialsModel()
        {
            this.MaxRetries = -1;
        }
    }
}