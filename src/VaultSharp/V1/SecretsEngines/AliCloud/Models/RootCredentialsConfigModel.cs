using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AliCloud.Models
{
    public class RootCredentialsConfigModel
    {
        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; }
    }
}