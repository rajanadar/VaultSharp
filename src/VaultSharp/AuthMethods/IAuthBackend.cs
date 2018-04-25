using VaultSharp.AuthMethods.AppRole;
using VaultSharp.AuthMethods.AWS;
using VaultSharp.AuthMethods.Cert;
using VaultSharp.AuthMethods.GitHub;
using VaultSharp.AuthMethods.Kubernetes;
using VaultSharp.AuthMethods.LDAP;
using VaultSharp.AuthMethods.Okta;
using VaultSharp.AuthMethods.RADIUS;
using VaultSharp.AuthMethods.Token;
using VaultSharp.AuthMethods.UserPass;

namespace VaultSharp.AuthMethods
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthBackend
    {
        /// <summary>
        /// 
        /// </summary>
        IAppRoleAuthBackend AppRole { get; }

        /// <summary>
        /// 
        /// </summary>
        IAWSAuthBackend AWS { get; }

        /// <summary>
        /// 
        /// </summary>
        // IGithubAuthBackend Github { get; }

        /// <summary>
        /// 
        /// </summary>
        IGitHubAuthBackend GoogleCloud { get; }

        /// <summary>
        /// 
        /// </summary>
        IKubernetesAuthBackend Kubernetes { get; }


        /// <summary>
        /// 
        /// </summary>
        ILDAPAuthBackend LDAP { get; }


        /// <summary>
        /// 
        /// </summary>
        IOktaAuthBackend Okta { get; }


        /// <summary>
        /// 
        /// </summary>
        IRADIUSAuthBackend RADIUS { get; }


        /// <summary>
        /// 
        /// </summary>
        ICertAuthBackend Cert { get; }


        /// <summary>
        /// 
        /// </summary>
        ITokenAuthBackend Token { get; }

        /// <summary>
        /// 
        /// </summary>
        IUserPassAuthBackend UserPass { get; }
    }
}