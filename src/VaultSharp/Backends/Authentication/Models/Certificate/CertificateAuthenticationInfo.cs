using System.Security.Cryptography.X509Certificates;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Models.Certificate
{
    /// <summary>
    /// Represents the login information for the Certificate Authentication backend.
    /// </summary>
    public class CertificateAuthenticationInfo : IAuthenticationInfo
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
                return AuthenticationBackendType.Certificate;
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
        /// Gets the client certificate.
        /// </summary>
        /// <value>
        /// The client certificate.
        /// </value>
        public X509Certificate2 ClientCertificate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateAuthenticationInfo" /> class.
        /// </summary>
        /// <param name="clientCertificate">The client certificate.</param>
        public CertificateAuthenticationInfo(X509Certificate2 clientCertificate) : this(AuthenticationBackendType.Certificate.Type, clientCertificate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateAuthenticationInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="clientCertificate">The client certificate.</param>
        public CertificateAuthenticationInfo(string mountPoint, X509Certificate2 clientCertificate)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(clientCertificate, "clientCertificate");

            MountPoint = mountPoint;
            ClientCertificate = clientCertificate;
        }
    }
}