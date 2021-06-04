using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate credentials request options.
    /// </summary>
    public class CertificateCredentialsRequestOptions : AbstractCertificateRequestOptions
    {
      
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the the format for marshaling the private key. 
        /// Defaults to der which will return either base64-encoded DER or PEM-encoded DER, 
        /// depending on the value of format. 
        /// The other option is pkcs8 which will return the key marshalled as PEM-encoded PKCS8.
        /// </summary>
        /// <value>
        /// The certificate format.
        /// </value>
        [JsonProperty("private_key_format")]
        public PrivateKeyFormat PrivateKeyFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateCredentialsRequestOptions"/> class.
        /// </summary>
        public CertificateCredentialsRequestOptions() : base()
        {
            PrivateKeyFormat = PrivateKeyFormat.der;
        }
    }
}