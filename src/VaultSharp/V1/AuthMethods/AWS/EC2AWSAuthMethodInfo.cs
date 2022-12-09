using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.AWS
{
    /// <summary>
    /// Represents the login information for the EC2 AWS Authentication backend.
    /// </summary>
    public class EC2AWSAuthMethodInfo : AbstractAWSAuthMethodInfo
    {
        /// <summary>
        /// Gets the AWS Auth method type.
        /// </summary>
        /// <value>
        /// The AWS auth type.
        /// </value>
        [JsonIgnore]
        public override AWSAuthMethodType AWSAuthMethodType { get { return AWSAuthMethodType.EC2; } }

        /// <summary>
        /// Gets the Base64 encoded EC2 instance identity document. 
        /// This needs to be supplied along with the signature parameter.
        /// </summary>
        /// <value>
        /// The Base64 encoded EC2 instance identity document signature.
        /// </value>
        [JsonProperty("identity")]
        public string Identity { get; }

        /// <summary>
        /// Gets the Base64 encoded SHA256 RSA signature of the instance identity document. 
        /// This needs to be supplied along with identity parameter when using the ec2 auth method.
        /// </summary>
        /// <value>
        /// The Identity signature.
        /// </value>
        [JsonProperty("signature")]
        public string Signature { get; }

        /// <summary>
        /// Gets the PKCS7 signature of the identity document with all \n characters removed.
        /// Either this needs to be set OR both identity and signature need to be set when using the ec2 auth method.
        /// </summary>
        /// <value>
        /// The PKCS7 signature.
        /// </value>
        [JsonProperty("pkcs7")]
        public string PKCS7 { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name = "pkcs7" >
        /// <para>[required/optional]</para>
        /// The PKCS7 signature of the identity document with all \n characters removed.
        /// Either this needs to be set OR both identity and signature need to be set when using the ec2 auth method.
        /// </param>
        /// <param name = "identity" >
        /// <para>[required/optional]</para>
        /// The Base64 encoded EC2 instance identity document. 
        /// This needs to be supplied along with the signature parameter.
        /// </param>
        /// <param name = "signature" >
        /// <para>[required/optional]</para>
        /// The Base64 encoded SHA256 RSA signature of the instance identity document. 
        /// This needs to be supplied along with identity parameter when using the ec2 auth method.
        /// </param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-accesslist 
        /// entry and returns the nonce back as part of auth metadata. 
        /// This value should be used with further login requests, to establish client authenticity. 
        /// Clients can choose to set a custom nonce if preferred, in which case, it is recommended 
        /// that clients provide a strong nonce. If a nonce is provided but with an empty value, 
        /// it indicates intent to disable reauthentication. Note that, when disallow_reauthentication option 
        /// is enabled on either the role or the role tag, the nonce holds no significance. 
        /// This is ignored unless using the ec2 auth method.
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.
        /// </param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// The name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login
        /// if using the ec2 auth method, or the "friendly name" (i.e., role name or username) 
        /// of the IAM principal authenticated.. 
        /// If a matching role is not found, login fails.
        /// </param>
        public EC2AWSAuthMethodInfo(string pkcs7, string identity, string signature, string nonce = null, string roleName = null)
            : this(AuthMethodType.AWS.Type, pkcs7, identity, signature, nonce, roleName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name = "pkcs7" >
        /// <para>[required/optional]</para>
        /// The PKCS7 signature of the identity document with all \n characters removed.
        /// Either this needs to be set OR both identity and signature need to be set when using the ec2 auth method.
        /// </param>
        /// <param name = "identity" >
        /// <para>[required/optional]</para>
        /// The Base64 encoded EC2 instance identity document. 
        /// This needs to be supplied along with the signature parameter.
        /// </param>
        /// <param name = "signature" >
        /// <para>[required/optional]</para>
        /// The Base64 encoded SHA256 RSA signature of the instance identity document. 
        /// This needs to be supplied along with identity parameter when using the ec2 auth method.
        /// </param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-accesslist 
        /// entry and returns the nonce back as part of auth metadata. 
        /// This value should be used with further login requests, to establish client authenticity. 
        /// Clients can choose to set a custom nonce if preferred, in which case, it is recommended 
        /// that clients provide a strong nonce. If a nonce is provided but with an empty value, 
        /// it indicates intent to disable reauthentication. Note that, when disallow_reauthentication option 
        /// is enabled on either the role or the role tag, the nonce holds no significance. 
        /// This is ignored unless using the ec2 auth method.
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.
        /// </param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// The name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login
        /// if using the ec2 auth method, or the "friendly name" (i.e., role name or username) 
        /// of the IAM principal authenticated.. 
        /// If a matching role is not found, login fails.
        /// </param>
        public EC2AWSAuthMethodInfo(string mountPoint, string pkcs7, string identity, string signature, string nonce = null, string roleName = null)
            : base(mountPoint, nonce, roleName)
        {
            if (string.IsNullOrWhiteSpace(identity) && string.IsNullOrWhiteSpace(signature))
            {
                Checker.NotNull(pkcs7, "pkcs7");
            }

            if (string.IsNullOrWhiteSpace(pkcs7))
            {
                Checker.NotNull(identity, "identity");
                Checker.NotNull(signature, "signature");
            }

            PKCS7 = pkcs7;
            Identity = identity;
            Signature = signature;
        }
    }
}