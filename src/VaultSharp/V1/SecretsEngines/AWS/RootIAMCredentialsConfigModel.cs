using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class RootIAMCredentialsConfigModel
    {
        [JsonPropertyName("max_retries")]
        public int MaxRetries { get; set; }

        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("iam_endpoint")]
        public string IAMEndpoint { get; set; }

        [JsonPropertyName("sts_endpoint")]
        public string STSEndpoint { get; set; }

        [JsonPropertyName("username_template")]
        public string UsernameTemplate { get; set; }
    }
}