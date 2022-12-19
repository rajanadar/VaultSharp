using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class RootIAMCredentialsConfigModel
    {
        [JsonProperty("max_retries")]
        public int MaxRetries { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("iam_endpoint")]
        public string IAMEndpoint { get; set; }

        [JsonProperty("sts_endpoint")]
        public string STSEndpoint { get; set; }

        [JsonProperty("username_template")]
        public string UsernameTemplate { get; set; }
    }
}