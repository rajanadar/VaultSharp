using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Sign Certificate request options.
    /// </summary>
    public class SignCertificatesRequestOptions
    {
        /// <summary>
        /// <para>[required]</para>
        ///  Specifies the PEM-encoded CSR
        /// </summary>
        /// <value>
        ///  Encoded CSR.
        /// </value>
        [JsonPropertyName("csr")]
        public string Csr { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the requested CN for the certificate. 
        /// If the CN is allowed by role policy, it will be issued.
        /// </summary>
        /// <value>
        /// The name of the common.
        /// </value>
        [JsonPropertyName("common_name")]
        public string CommonName { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the requested Subject Alternative Names, in a comma-delimited list. 
        /// These can be host names or email addresses; they will be parsed into their respective fields. 
        /// If any requested names do not match role policy, the entire request will be denied.
        /// </summary>
        /// <value>
        /// The subject alternative names.
        /// </value>
        [JsonPropertyName("alt_names")]
        public string SubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the requested IP Subject Alternative Names, in a comma-delimited list. 
        /// Only valid if the role allows IP SANs (which is the default).
        /// </summary>
        /// <value>
        /// The ip subject alternative names.
        /// </value>
        [JsonPropertyName("ip_sans")]
        public string IPSubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the requested URI Subject Alternative Names, in a comma-delimited list.
        /// </summary>
        /// <value>
        /// The uri subject alternative names.
        /// </value>
        [JsonPropertyName("uri_sans")]
        public string URISubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the custom OID/UTF8-string SANs. 
        /// These must match values specified on the role in allowed_other_sans (globbing allowed). 
        /// The format is the same as OpenSSL: [oid];[type]:[value] where the only current valid type is UTF8. 
        /// This can be a comma-delimited list or a JSON string slice.
        /// </summary>
        /// <value>
        /// The other subject alternative names.
        /// </value>
        [JsonPropertyName("other_sans")]
        public string OtherSubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the requested Time To Live. 
        /// Cannot be greater than the role's max_ttl value. 
        /// If not provided, the role's ttl value will be used. 
        /// Note that the role values default to system values if not explicitly set.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonPropertyName("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the certificate format for returned data. 
        /// Can be pem or der; defaults to pem. 
        /// If der, the output is base64 encoded..
        /// </summary>
        /// <value>
        /// The certificate format.
        /// </value>
        [JsonPropertyName("format")]
        public CertificateFormat CertificateFormat { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [exclude common name from subject alternative names].
        /// If set, the given common name will not be included in DNS or Email Subject Alternate Names (as appropriate). 
        /// Useful if the CN is not a hostname or email address, but is instead some human-readable identifier.
        /// </summary>
        /// <value>
        /// <c>true</c> if [exclude common name from subject alternative names]; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("exclude_cn_from_sans")]
        public bool ExcludeCommonNameFromSubjectAlternativeNames { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignCertificatesRequestOptions"/> class.
        /// </summary>
        public SignCertificatesRequestOptions()
        {
            CertificateFormat = CertificateFormat.pem;
        }
    }
}