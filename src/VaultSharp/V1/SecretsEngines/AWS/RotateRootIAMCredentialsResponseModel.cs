using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class RotateRootIAMCredentialsResponseModel
    {
        [JsonPropertyName("access_key")]
        public string NewAccessKey { get; set; }
    }
}