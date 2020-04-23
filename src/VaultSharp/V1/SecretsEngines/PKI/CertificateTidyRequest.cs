using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate Tidy Request.
    /// </summary>
    public class CertificateTidyRequest
    {
        /// <summary>
        /// Specifies whether to tidy up the certificate store. Defaults to false.
        /// </summary>
        [JsonProperty("tidy_cert_store")]
        public bool TidyCertStore { get; set; }

        /// <summary>
        /// Set to true to expire all revoked and expired certificates, removing them both from the CRL and from storage. 
        /// The CRL will be rotated if this causes any values to be removed.
        /// </summary>
        [JsonProperty("tidy_revoked_certs")]
        public bool TidyRevokedCerts { get; set; }

        /// <summary>
        /// Specifies A duration (given as an integer number of seconds or a string; defaults to 72h) 
        /// Used as a safety buffer to ensure certificates are not expunged prematurely; 
        /// as an example, this can keep certificates from being removed from the CRL that, due to clock skew,
        /// might still be considered valid on other hosts. 
        /// For a certificate to be expunged, the time must be after the expiration time of the certificate 
        /// (according to the local clock) plus the duration of safety_buffer.
        /// </summary>
        [JsonProperty("safety_buffer")]
        public string SafetyBuffer { get; set; }

        public CertificateTidyRequest()
        {
            TidyCertStore = false;
            TidyRevokedCerts = false;
            SafetyBuffer = "72h";
        }
    }
}