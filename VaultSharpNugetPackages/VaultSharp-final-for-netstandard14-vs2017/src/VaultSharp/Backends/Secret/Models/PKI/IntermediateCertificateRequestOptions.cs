using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the intermediate CA Certificate request options.
    /// </summary>
    public class IntermediateCertificateRequestOptions : CertificateRequestOptions
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
        /// Gets or sets the maximum length of the path.
        /// If set, the maximum path length to encode in the generated certificate. 
        /// Defaults to -1, which means no limit, unless the signing certificate has a maximum path length set, 
        /// in which case the path length is set to one less than that of the signing certificate. 
        /// A limit of 0 means a literal path length of zero.
        /// </summary>
        /// <value>
        /// The maximum length of the path.
        /// </value>
        [JsonProperty("max_path_length")]
        public int MaximumPathLength { get; set; }

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

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [use certificate signing request values].
        /// If set to true, then: 
        /// 1) Subject information, including names and alternate names, will be preserved from the CSR 
        /// rather than using the values provided in the other parameters to this path; 
        /// 2) Any key usages (for instance, non-repudiation) requested in the CSR will be added to the basic set of key 
        /// usages used for CA certs signed by this path; 
        /// 3) Extensions requested in the CSR will be copied into the issued certificate.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use certificate signing request values]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("use_csr_values")]
        public bool UseCertificateSigningRequestValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntermediateCertificateRequestOptions"/> class.
        /// </summary>
        public IntermediateCertificateRequestOptions()
        {
            MaximumPathLength = -1;
            UseCertificateSigningRequestValues = false;
        }
    }
}