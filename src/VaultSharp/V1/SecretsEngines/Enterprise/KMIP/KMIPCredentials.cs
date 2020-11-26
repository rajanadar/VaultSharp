using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KMIP
{
    /// <summary>
    /// Represents the KMIP credentials.
    /// </summary>
    public class KMIPCredentials
    {
        /// <summary>
        /// Gets or sets the ca chain content.
        /// </summary>
        /// <value>
        /// The content of the ca chain.
        /// </value>
        [JsonProperty("ca_chain")]
        public List<string> CAChainContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the certificate.
        /// </summary>
        /// <value>
        /// The content of the certificate.
        /// </value>
        [JsonProperty("certificate")]
        public string CertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        [JsonProperty("private_key")]
        public string PrivateKeyContent { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }
    }
}