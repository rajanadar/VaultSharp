using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Kubernetes
{
    /// <summary>
    /// Represents the Kubernetes credentials.
    /// </summary>
    public class KubernetesCredentials
    {
        [JsonProperty("service_account_name")]
        public string ServiceAccountName { get; set; }

        [JsonProperty("service_account_namespace")]
        public string ServiceAccountNamespace { get; set; }

        [JsonProperty("service_account_token")]
        public string ServiceAccountToken { get; set; }
    }
}