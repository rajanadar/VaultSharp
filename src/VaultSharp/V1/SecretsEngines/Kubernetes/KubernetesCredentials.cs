using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Kubernetes
{
    /// <summary>
    /// Represents the Kubernetes credentials.
    /// </summary>
    public class KubernetesCredentials
    {
        [JsonPropertyName("service_account_name")]
        public string ServiceAccountName { get; set; }

        [JsonPropertyName("service_account_namespace")]
        public string ServiceAccountNamespace { get; set; }

        [JsonPropertyName("service_account_token")]
        public string ServiceAccountToken { get; set; }
    }
}