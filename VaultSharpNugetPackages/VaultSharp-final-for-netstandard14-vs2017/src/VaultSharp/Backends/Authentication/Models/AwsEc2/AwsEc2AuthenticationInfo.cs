using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.AwsEc2
{
    /// <summary>
    /// Represents the login information for the AwsEc2 Authentication backend.
    /// </summary>
    public class AwsEc2AuthenticationInfo : IAuthenticationInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public AuthenticationBackendType AuthenticationBackendType
        {
            get
            {
                return AuthenticationBackendType.AwsEc2;
            }
        }

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// Gets the PKCS7 signature of the identity document with all \n characters removed.
        /// </summary>
        /// <value>
        /// The PKCS7 signature.
        /// </value>
        public string Pkcs7 { get; }

        /// <summary>
        /// Gets the nonce created by a client of this backend. 
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        public string Nonce { get; }

        /// <summary>
        /// Gets the name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login. 
        /// If a matching role is not found, login fails.
        /// </summary>
        /// <value>
        /// The role name.
        /// </value>
        public string RoleName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsEc2AuthenticationInfo"/> class.
        /// </summary>
        /// <param name="pkcs7">
        /// <para>[required]</para>
        /// The PKCS7 signature of the identity document with all \n characters removed..</param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce created by a client of this backend. 
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.</param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// The name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login. 
        /// If a matching role is not found, login fails.
        /// </param>
        public AwsEc2AuthenticationInfo(string pkcs7, string nonce = null, string roleName = null)
            : this(AuthenticationBackendDefaultPaths.AwsEc2, pkcs7, nonce, roleName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsEc2AuthenticationInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">
        /// <para>[required]</para>
        /// The mount point.</param>
        /// <param name="pkcs7">
        /// <para>[required]</para>
        /// The PKCS7 signature of the identity document with all \n characters removed..</param>
        /// <param name="nonce">
        /// <para>[required/optional]</para>
        /// The nonce created by a client of this backend. 
        /// When <see cref="disallowReauthentication"/> option is enabled on either the role 
        /// or the role tag, then nonce parameter is optional. 
        /// It is a required parameter otherwise.</param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// The name of the role against which the login is being attempted. 
        /// If role is not specified, then the login endpoint looks for a role bearing 
        /// the name of the AMI ID of the EC2 instance that is trying to login. 
        /// If a matching role is not found, login fails.
        /// </param>
        public AwsEc2AuthenticationInfo(string mountPoint, string pkcs7, string nonce = null, string roleName = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(pkcs7, "pkcs7");

            MountPoint = mountPoint;
            Pkcs7 = pkcs7;
            Nonce = nonce;
            RoleName = roleName;
        }
    }
}