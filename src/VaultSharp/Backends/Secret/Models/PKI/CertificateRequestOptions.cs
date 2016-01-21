using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the options for a certificate request.
    /// </summary>
    public abstract class CertificateRequestOptions
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the requested CN for the certificate.
        /// </summary>
        /// <value>
        /// The name of the common.
        /// </value>
        [JsonProperty("common_name")]
        public string CommonName { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the requested Subject Alternative Names, in a comma-delimited list. 
        /// These can be host names or email addresses; they will be parsed into their respective fields.
        /// </summary>
        /// <value>
        /// The subject alternative names.
        /// </value>
        [JsonProperty("alt_names")]
        public string SubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the requested IP Subject Alternative Names, in a comma-delimited list.
        /// </summary>
        /// <value>
        /// The ip subject alternative names.
        /// </value>
        [JsonProperty("ip_sans")]
        public string IPSubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the certificate format for returned data.
        /// Can be pem or der; defaults to pem. If der, the output is base64 encoded.
        /// </summary>
        /// <value>
        /// The certificate format.
        /// </value>
        [JsonProperty("format")]
        public CertificateFormat CertificateFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRequestOptions"/> class.
        /// </summary>
        protected CertificateRequestOptions()
        {
            CertificateFormat = CertificateFormat.pem;
        }
    }
}