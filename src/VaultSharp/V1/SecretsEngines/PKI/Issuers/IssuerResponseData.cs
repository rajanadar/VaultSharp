using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public class IssuerResponseData
    {
        [JsonPropertyName("certificate")]
        public string Certificate { get; set; }

        [JsonPropertyName("expiration")]
        public int Expiration { get; set; }

        [JsonPropertyName("issuer_id")]
        public string IssuerId { get; set; }

        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; }

        [JsonPropertyName("issuing_ca")]
        public string IssuingCa { get; set; }

        [JsonPropertyName("key_id")]
        public string KeyId { get; set; }

        [JsonPropertyName("key_name")]
        public string KeyName { get; set; }

        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }
    }
}