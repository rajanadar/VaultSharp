using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the generated Certificate credentials.
    /// </summary>
    public class CertificateCredentials : CertificateData
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
    }
}