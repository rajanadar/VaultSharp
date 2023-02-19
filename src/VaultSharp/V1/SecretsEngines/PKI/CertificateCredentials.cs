using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the generated Certificate credentials.
    /// </summary>
    public class CertificateCredentials : AbstractIssuedCertificateData
    {
        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        [JsonPropertyName("private_key")]
        public string PrivateKeyContent { get; set; }

        /// <summary>
        /// Gets or sets the type of the private key.
        /// </summary>
        /// <value>
        /// The type of the private key.
        /// </value>
        [JsonPropertyName("private_key_type")]
        public CertificateKeyType PrivateKeyType { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        [JsonPropertyName("expiration")]
        public long Expiration { get; set; }
    }
}