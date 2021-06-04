using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate credentials request options.
    /// </summary>
    public class SignCertificatesRequestOptions : AbstractCertificateRequestOptions
    {

        /// <summary>
        /// <para>[required]</para>
        ///  Specifies the PEM-encoded CSR
        /// </summary>
        /// <value>
        ///  Encoded CSR.
        /// </value>
        [JsonProperty("csr")]
        public string Csr { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignCertificatesRequestOptions"/> class.
        /// </summary>
        public SignCertificatesRequestOptions() : base() { }

    }
}