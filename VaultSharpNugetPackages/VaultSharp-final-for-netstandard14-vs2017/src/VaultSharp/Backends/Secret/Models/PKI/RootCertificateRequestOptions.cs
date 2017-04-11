using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the root CA Certificate request options.
    /// </summary>
    public class RootCertificateRequestOptions : CertificateRequestOptions
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the desired key type; must be rsa or ec. 
        /// Defaults to rsa.
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        [JsonProperty("key_type")]
        public CertificateKeyType KeyType { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the number of bits to use. 
        /// Defaults to 2048. 
        /// Must be changed to a valid value if the <see cref="CertificateKeyType"/> is ec.
        /// </summary>
        /// <value>
        /// The number of key bits.
        /// </value>
        [JsonProperty("key_bits")]
        public int KeyBits { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [export private key].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export private key]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool ExportPrivateKey { get; set; }

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
        /// Initializes a new instance of the <see cref="RootCertificateRequestOptions"/> class.
        /// </summary>
        public RootCertificateRequestOptions()
        {
            KeyType = CertificateKeyType.rsa;
            KeyBits = 2048;
            MaximumPathLength = -1;
        }
    }
}