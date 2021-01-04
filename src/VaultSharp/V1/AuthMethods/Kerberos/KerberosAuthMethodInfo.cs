using System.Net;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Kerberos
{
    /// <summary>
    /// Represents the login information for the Kerberos Authentication backend.
    /// </summary>
    public class KerberosAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        public override AuthMethodType AuthMethodType => AuthMethodType.Kerberos;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// Credentials to use for Kerberos authentication.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        public ICredentials Credentials { get; }

        /// <summary>
        /// Flag to indicate if the credentials should be cached.
        /// Defaults to true.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        public bool PreAuthenticate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerberosAuthMethodInfo"/> class.
        /// </summary>
        public KerberosAuthMethodInfo() 
            : this(AuthMethodType.Kerberos.Type, CredentialCache.DefaultCredentials, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerberosAuthMethodInfo"/> class.
        /// </summary>
        public KerberosAuthMethodInfo(ICredentials credentials) 
            : this(AuthMethodType.Kerberos.Type, credentials, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerberosAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="credentials">The credential to use.</param>
        public KerberosAuthMethodInfo(string mountPoint, ICredentials credentials)
            : this (mountPoint, credentials, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerberosAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="credentials">The credential to use.</param>
        /// <param name="preAuthenticate">The flag to cache credentials.</param>
        public KerberosAuthMethodInfo(string mountPoint, ICredentials credentials, bool preAuthenticate)
        {
            Checker.NotNull(mountPoint, nameof(mountPoint));
            Checker.NotNull(credentials, nameof(credentials));

            MountPoint = mountPoint;
            Credentials = credentials;
            PreAuthenticate = preAuthenticate;
        }
    }
}
