using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents a new certificate signing request set of options.
    /// </summary>
    public class NewCertificateSigningRequestOptions : CertificateRequestOptions
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the pem encoded certificate signing request.
        /// </summary>
        /// <value>
        /// The pem encoded certificate signing request.
        /// </value>
        [JsonProperty("csr")]
        public string PemEncodedCertificateSigningRequest { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the Time To Live (after which the certificate will be expired). 
        /// This cannot be larger than the mount max (or, if not set, the system max).
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [exclude common name from subject alternative names].
        /// If set, the given common name will not be included in DNS or Email Subject Alternate Names (as appropriate). 
        /// Useful if the CN is not a hostname or email address, but is instead some human-readable identifier.
        /// </summary>
        /// <value>
        /// <c>true</c> if [exclude common name from subject alternative names]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("exclude_cn_from_sans")]
        public bool ExcludeCommonNameFromSubjectAlternativeNames { get; set; }
    }
}