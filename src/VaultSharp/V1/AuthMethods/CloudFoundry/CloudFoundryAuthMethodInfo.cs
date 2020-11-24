using System;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.CloudFoundry
{
    /// <summary>
    /// 
    /// </summary>
    public class CloudFoundryAuthMethodInfo : AbstractAuthMethodInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public override AuthMethodType AuthMethodType => AuthMethodType.CloudFoundry;

        /// <summary>
        /// Gets the mount point.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        public string MountPoint { get; }

        /// <summary>
        /// [required]
        /// Gets the name of the role against which the login is being attempted.
        /// </summary>
        /// <value>
        /// The role name.
        /// </value>
        public string RoleName { get; }

        /// <summary>
        /// [required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_CERT.
        /// </summary>
        public string CFInstanceCertContent { get; }

        /// <summary>
        /// [required]
        /// The signature.
        /// Please see https://gist.github.com/rajanadar/84769efeca64e0128d7a8a627b7bb4db
        /// </summary>
        public string Signature { get; }

        /// <summary>
        /// [required]
        /// The datetime used in the signature.
        /// </summary>
        public DateTime SignatureDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFoundryAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="instanceCertContent">[required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_CERT.
        /// </param>
        /// <param name="signatureDateTime">[required]
        /// The datetime used in the signature.
        /// </param>
        /// <param name="signature">[required]
        /// The signature.
        /// </param>
        public CloudFoundryAuthMethodInfo(string roleName, string instanceCertContent, DateTime signatureDateTime, string signature)
            : this(AuthMethodType.CloudFoundry.Type, roleName, instanceCertContent, signatureDateTime, signature)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudFoundryAuthMethodInfo"/> class.
        /// </summary>
        /// <param name="mountPoint">The mount point.</param>
        /// <param name="roleName">[required]
        /// The name of the role against which the login is being attempted.
        /// </param>
        /// <param name="instanceCertContent">[required]
        /// The full body of the file available at the path denoted by CF_INSTANCE_CERT.
        /// </param>
        /// <param name="signatureDateTime">[required]
        /// The datetime used in the signature.
        /// </param>
        /// <param name="signature">[required]
        /// The signature.
        /// </param>
        public CloudFoundryAuthMethodInfo(string mountPoint, string roleName, string instanceCertContent, DateTime signatureDateTime, string signature)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(instanceCertContent, "instanceCertContent");
            Checker.NotNull(signature, "signature");

            MountPoint = mountPoint;
            RoleName = roleName;
            CFInstanceCertContent = instanceCertContent;
            SignatureDateTime = signatureDateTime;
            Signature = signature;
        }
    }
}
