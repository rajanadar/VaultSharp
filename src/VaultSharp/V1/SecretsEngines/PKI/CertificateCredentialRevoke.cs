using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate revocation info.
    /// </summary>
    public class CertificateCredentialRevoke
    {
        /// <summary>
        /// Gets or sets the revocation time.
        /// </summary>
        /// <value>
        /// The revocation time.
        /// </value>
        [JsonProperty("revocation_time")]
        public int RevocationTime { get; set; }
    }
}