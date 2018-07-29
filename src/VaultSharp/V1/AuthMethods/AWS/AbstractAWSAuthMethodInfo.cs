using Newtonsoft.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.AWS
{
    /// <summary>
    /// Represents the login information for the AWS Authentication backend.
    /// </summary>
    public abstract class AbstractAWSAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the AWS Auth method type.
        /// </summary>
        /// <value>
        /// The AWS auth type.
        /// </value>
        [JsonIgnore]
        public abstract AWSAuthMethodType AWSAuthMethodType { get; }

        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        [JsonIgnore]
        public override AuthMethodType AuthMethodType => AuthMethodType.AWS;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        [JsonIgnore]
        public string MountPoint { get; }

        /// <summary>
        /// Gets the name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login
        /// if using the ec2 auth method, or the "friendly name" (i.e., role name or username) 
        /// of the IAM principal authenticated.. 
        /// If a matching role is not found, login fails.
        /// </summary>
        /// <value>
        /// The role name.
        /// </value>
        [JsonProperty("role")]
        public string RoleName { get; }

        /// <summary>
        /// Gets the nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-whitelist 
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
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        [JsonProperty("nonce")]
        public string Nonce { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-whitelist 
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
        protected AbstractAWSAuthMethodInfo(string nonce = null, string roleName = null)
            : this(AuthMethodType.AWS.Type, nonce, roleName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAWSAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">
        /// <para>[required]</para>
        /// The mount point.</param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce to be used for subsequent login requests. 
        /// If this parameter is not specified at all and if reauthentication is allowed, 
        /// then the method will generate a random nonce, attaches it to the instance's identity-whitelist 
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
        protected AbstractAWSAuthMethodInfo(string mountPoint, string nonce = null, string roleName = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            MountPoint = mountPoint;
            Nonce = nonce;
            RoleName = roleName;
        }
    }
}