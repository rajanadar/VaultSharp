using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate Auto Tidy Request.
    /// </summary>
    public class CertificateAutoTidyRequest 
    {

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true; // If a caller is calling our API, make this true.

        [JsonPropertyName("interval_duration")]
        public string IntervalDuration { get; set; } = "12h";

        /// <summary>
        /// Specifies whether to tidy up the certificate store. Defaults to false.
        /// </summary>
        [JsonPropertyName("tidy_cert_store")]
        public bool TidyCertStore { get; set; } = false;

        /// <summary>
        /// Set to true to expire all revoked and expired certificates, removing them both from the CRL and from storage. 
        /// The CRL will be rotated if this causes any values to be removed.
        /// </summary>
        [JsonPropertyName("tidy_revoked_certs")]
        public bool TidyRevokedCerts { get; set; } = false;

        [JsonPropertyName("tidy_revoked_cert_issuer_associations")]
        public bool TidyRevokedCertIssuerAssociations { get; set; } = false;

        [JsonPropertyName("tidy_expired_issuers")]
        public bool TidyExpiredIssuers { get; set; } = false;

        /// <summary>
        /// Specifies A duration (given as an integer number of seconds or a string; defaults to 72h) 
        /// Used as a safety buffer to ensure certificates are not expunged prematurely; 
        /// as an example, this can keep certificates from being removed from the CRL that, due to clock skew,
        /// might still be considered valid on other hosts. 
        /// For a certificate to be expunged, the time must be after the expiration time of the certificate 
        /// (according to the local clock) plus the duration of safety_buffer.
        /// </summary>
        [JsonPropertyName("safety_buffer")]
        public string SafetyBuffer { get; set; } = "72h";

        [JsonPropertyName("issuer_safety_buffer")]
        public string IssuerSafetyBuffer { get; set; } = "8760h"; // 365*24 hrs
    }
}