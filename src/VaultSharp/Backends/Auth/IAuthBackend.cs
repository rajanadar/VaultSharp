using VaultSharp.Backends.Auth.AppRole;
using VaultSharp.Backends.Auth.AWS;
using VaultSharp.Backends.Auth.Cert;
using VaultSharp.Backends.Auth.GitHub;
using VaultSharp.Backends.Auth.Kubernetes;
using VaultSharp.Backends.Auth.LDAP;
using VaultSharp.Backends.Auth.Okta;
using VaultSharp.Backends.Auth.RADIUS;
using VaultSharp.Backends.Auth.Token;
using VaultSharp.Backends.Auth.UserPass;

namespace VaultSharp.Backends.Auth
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