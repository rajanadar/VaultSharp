using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the options for tidying up the PKI backend.
    /// </summary>
    public class TidyRequestOptions
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Whether to tidy up the certificate store. Defaults to false.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("tidy_cert_store")]
        public bool TidyUpCertificateStore { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Whether to tidy up the revocation list (CRL). Defaults to false.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("tidy_revocation_list")]
        public bool TidyUpRevocationList { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        ///  A duration (given as an integer number of seconds or a string; defaults to 72h) used as a safety 
        /// buffer to ensure certificates are not expunged prematurely; 
        /// as an example, this can keep certificates from being removed from the CRL that, 
        /// due to clock skew, might still be considered valid on other hosts. 
        /// For a certificate to be expunged, the time must be after the expiration 
        /// time of the certificate (according to the local clock) plus the duration of <see cref="SafetyBufferDuration"/>.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("safety_buffer")]
        public string SafetyBufferDuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TidyRequestOptions"/> class.
        /// </summary>
        public TidyRequestOptions()
        {
            SafetyBufferDuration = "72h";
        }
    }
}