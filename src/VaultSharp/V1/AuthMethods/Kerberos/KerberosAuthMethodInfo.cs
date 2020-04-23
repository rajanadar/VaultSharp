using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
        /// Initializes a new instance of the <see cref="KerberosAuthMethodInfo"/> class.
        /// </summary>
        public KerberosAuthMethodInfo() : this(AuthMethodType.Kerberos.Type, CredentialCache.DefaultNetworkCredentials)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerberosAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="credentials">The credential to use.</param>
        public KerberosAuthMethodInfo(string mountPoint, ICredentials credentials)
        {
            Checker.NotNull(mountPoint, nameof(mountPoint));
            Checker.NotNull(credentials, nameof(credentials));

            MountPoint = mountPoint;
            Credentials = credentials;
        }
    }
}
