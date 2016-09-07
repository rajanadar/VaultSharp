using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the options for a CSR.
    /// </summary>
    public class CertificateSigningRequestOptions : CertificateRequestOptions
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
        /// Gets or sets a value indicating whether [export private key].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export private key]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool ExportPrivateKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateSigningRequestOptions"/> class.
        /// </summary>
        public CertificateSigningRequestOptions()
        {
            KeyType = CertificateKeyType.rsa;
            KeyBits = 2048;
        }
    }
}