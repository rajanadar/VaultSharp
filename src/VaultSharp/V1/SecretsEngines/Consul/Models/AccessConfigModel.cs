using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Consul.Models
{
    public class AccessConfigModel
    {
        [JsonProperty("address")]
        public string ConsulAddressWithPort { get; set; }

        [JsonProperty("scheme")]
        public string UrlScheme { get; set; }

        [JsonProperty("ca_cert")]
        public string X509PEMEncodedServerCertificate { get; set; }

        [JsonProperty("client_cert")]
        public string X509PEMEncodedTLSClientCertificate { get; set; }

        [JsonProperty("client_key")]
        public string X509PEMEncodedTLSClientKey { get; set; }

        [JsonProperty("token")]
        public string ConsulToken { get; set; }
    }
}