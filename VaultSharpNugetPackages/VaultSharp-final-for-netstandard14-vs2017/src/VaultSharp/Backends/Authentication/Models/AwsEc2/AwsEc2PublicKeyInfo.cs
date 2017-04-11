using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.AwsEc2
{
    /// <summary>
    /// Represents the AWS EC2 Client access credentials.
    /// </summary>
    public class AwsEc2PublicKeyInfo
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the name of the certificate.
        /// </summary>
        /// <value>
        /// The name of the certificate.
        /// </value>
        [JsonIgnore]
        public string CertificateName { get; set; }

        /// <summary>
        /// <para>[required]</para> 
        /// Gets or sets the AWS Public key required to verify PKCS7 signature of the EC2 instance metadata.
        /// </summary>
        /// <value>
        /// The aws public key.
        /// </value>
        [JsonProperty("aws_public_cert")]
        public string AwsPublicKey { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the type of document which can be verified using the given certificate. 
        /// The PKCS#7 document will have a DSA digest and the identity signature will have an RSA signature, 
        /// and accordingly the public certificates to verify those also vary. Defaults to <see cref="AwsEc2DocumentType.pkcs7"/>.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        [JsonProperty("type")]
        public AwsEc2DocumentType DocumentType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsEc2PublicKeyInfo"/> class.
        /// </summary>
        public AwsEc2PublicKeyInfo()
        {
            DocumentType = AwsEc2DocumentType.pkcs7;
        }
    }
}