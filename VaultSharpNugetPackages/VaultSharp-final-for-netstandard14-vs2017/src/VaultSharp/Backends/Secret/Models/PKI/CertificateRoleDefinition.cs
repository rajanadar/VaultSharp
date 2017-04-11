using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the Certificate role definition.
    /// </summary>
    public class CertificateRoleDefinition
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the Time To Live value provided as a string duration with time suffix. 
        /// Hour is the largest suffix. 
        /// If not set, uses the system default value or the value of <see cref="MaximumTimeToLive"/>, whichever is shorter.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the maximum Time To Live provided as a string duration with time suffix. 
        /// Hour is the largest suffix. 
        /// If not set, defaults to the system maximum lease TTL.
        /// </summary>
        /// <value>
        /// The maximum time to live.
        /// </value>
        [JsonProperty("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [allow localhost].
        /// If set, clients can request certificates for localhost as one of the requested common names. 
        /// This is useful for testing and to allow clients on a single host to talk securely. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow localhost]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("allow_localhost")]
        public bool AllowLocalhost { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the designated domains of the role, provided as a comma-separated list. 
        /// This is used with the <see cref="AllowBareDomains"/> and <see cref="AllowSubdomains"/> options. 
        /// There is no default.
        /// </summary>
        /// <value>
        /// The allowed domains.
        /// </value>
        [JsonProperty("allowed_domains")]
        public string AllowedDomains { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [allow bare domains].
        /// If set, clients can request certificates matching the value of the actual domains themselves; 
        /// e.g. if a configured domain set with <see cref="AllowedDomains"/> is example.com, 
        /// this allows clients to actually request a certificate containing the name example.com as one of the DNS values on the final certificate. 
        /// In some scenarios, this can be considered a security risk. 
        /// Defaults to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow bare domains]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("allow_bare_domains")]
        public bool AllowBareDomains { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [allow subdomains].
        /// If set, clients can request certificates with CNs that are subdomains of the CNs allowed by the other role options. 
        /// This includes wildcard subdomains. 
        /// For example, an <see cref="AllowedDomains"/> value of example.com with this option set to true will allow foo.example.com and bar.example.com 
        /// as well as *.example.com. 
        /// This is redundant when using the <see cref="AllowAnyName"/> option. 
        /// Defaults to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow subdomains]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("allow_subdomains")]
        public bool AllowSubdomains { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [allow any name].
        /// If set, clients can request any CN. 
        /// Useful in some circumstances, but make sure you understand whether it is appropriate for your installation before enabling it. 
        /// Defaults to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow any name]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("allow_any_name")]
        public bool AllowAnyName { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [enforce hostnames].
        /// If set, only valid host names are allowed for CNs, DNS SANs, and the host part of email addresses. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enforce hostnames]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("enforce_hostnames")]
        public bool EnforceHostnames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [allow ip subject alternative names].
        /// If set, clients can request IP Subject Alternative Names. 
        /// No authorization checking is performed except to verify that the given values are valid IP addresses. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        /// <c>true</c> if [allow ip subject alternative names]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("allow_ip_sans")]
        public bool AllowIPSubjectAlternativeNames { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [server flag].
        /// If set, certificates are flagged for server use. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [server flag]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("server_flag")]
        public bool ServerFlag { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [client flag].
        /// If set, certificates are flagged for client use. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [client flag]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("client_flag")]
        public bool ClientFlag { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [code signing flag].
        /// If set, certificates are flagged for code signing use. 
        /// Defaults to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [code signing flag]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("code_signing_flag")]
        public bool CodeSigningFlag { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [email protection flag].
        /// If set, certificates are flagged for email protection use. 
        /// Defaults to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [email protection flag]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("email_protection_flag")]
        public bool EmailProtectionFlag { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the type of key to generate for generated private keys. 
        /// Currently, rsa and ec are supported. 
        /// Defaults to rsa.
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        [JsonProperty("key_type")]
        public CertificateKeyType KeyType { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the number of bits to use for the generated keys. 
        /// Defaults to 2048; this will need to be changed for ec keys. 
        /// See https://golang.org/pkg/crypto/elliptic/#Curve for an overview of allowed bit lengths for ec.
        /// </summary>
        /// <value>
        /// The key bits.
        /// </value>
        [JsonProperty("key_bits")]
        public int KeyBits { get; set; }

        /// <summary>
        /// Gets or sets the allowed key usage constraint on issued certificates. 
        /// This is a comma-separated string; valid values can be found at https://golang.org/pkg/crypto/x509/#KeyUsage -- 
        /// simply drop the KeyUsage part of the value. 
        /// Values are not case-sensitive. To specify no key usage constraints, set this to an empty string. 
        /// Defaults to DigitalSignature,KeyAgreement,KeyEncipherment.
        /// </summary>
        /// <value>
        /// The key usage.
        /// </value>
        public string KeyUsage { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [use certificate signing request common name].
        /// If set, when used with the CSR signing API, the common name in the CSR will be used instead of taken from the input data. 
        /// This does not include any requested SANs in the CSR. 
        /// Defaults to false.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use certificate signing request common name]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("use_csr_common_name")]
        public bool UseCertificateSigningRequestCommonName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRoleDefinition"/> class.
        /// </summary>
        public CertificateRoleDefinition()
        {
            AllowLocalhost = true;
            AllowBareDomains = false;
            AllowSubdomains = false;
            AllowAnyName = false;
            EnforceHostnames = true;
            AllowIPSubjectAlternativeNames = true;
            ServerFlag = true;
            ClientFlag = true;
            CodeSigningFlag = false;
            EmailProtectionFlag = false;
            KeyType = CertificateKeyType.rsa;
            KeyBits = 2048;
            UseCertificateSigningRequestCommonName = false;
            KeyUsage = "DigitalSignature,KeyAgreement,KeyEncipherment";
        }
    }
}