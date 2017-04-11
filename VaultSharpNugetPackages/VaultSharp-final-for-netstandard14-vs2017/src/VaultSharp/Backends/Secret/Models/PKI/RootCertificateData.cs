using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the generated root Certificate.
    /// </summary>
    public class RootCertificateData : CertificateData
    {
        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        [JsonProperty("private_key")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the private key.
        /// </summary>
        /// <value>
        /// The type of the private key.
        /// </value>
        [JsonProperty("private_key_type")]
        public CertificateKeyType PrivateKeyType { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        [JsonProperty("expiration")]
        public int Expiration { get; set; }
    }
}