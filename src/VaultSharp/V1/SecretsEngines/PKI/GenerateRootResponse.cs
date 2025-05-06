using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class GenerateRootResponse
    {
        /// <summary>
        /// The generated self-signed CA certificate.
        /// </summary>
        [JsonPropertyName("certificate")]
        public string Certificate { get; set; }

        /// <summary>
        /// The expiration of the given issuer.
        /// </summary>
        [JsonPropertyName("expiration")]
        public long Expiration { get; set; }

        /// <summary>
        /// The ID of the issuer
        /// </summary>
        [JsonPropertyName("issuer_id")]
        public string IssuerId { get; set; }

        /// <summary>
        /// The name of the issuer.
        /// </summary>
        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; }

        /// <summary>
        /// The issuing certificate authority.
        /// </summary>
        [JsonPropertyName("issuing_ca")]
        public string IssuingCA { get; set; }

        /// <summary>
        /// The ID of the key.
        /// </summary>
        [JsonPropertyName("key_id")]
        public string KeyId { get; set; }

        /// <summary>
        /// The key name if given.
        /// </summary>
        [JsonPropertyName("key_name")]
        public string KeyName { get; set; }

        /// <summary>
        /// The private key if exported was specified
        /// </summary>
        [JsonPropertyName("private_key")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// The requested Subject's named serial number.
        /// </summary>
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }
    }
}
