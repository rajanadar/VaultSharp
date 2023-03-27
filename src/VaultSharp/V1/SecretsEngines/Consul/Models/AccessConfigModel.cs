using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
    public class AccessConfigModel
    {
        [JsonPropertyName("address")]
        public string ConsulAddressWithPort { get; set; }

        [JsonPropertyName("scheme")]
        public string UrlScheme { get; set; }

        [JsonPropertyName("ca_cert")]
        public string X509PEMEncodedServerCertificate { get; set; }

        [JsonPropertyName("client_cert")]
        public string X509PEMEncodedTLSClientCertificate { get; set; }

        [JsonPropertyName("client_key")]
        public string X509PEMEncodedTLSClientKey { get; set; }

        [JsonPropertyName("token")]
        public string ConsulToken { get; set; }
    }
}