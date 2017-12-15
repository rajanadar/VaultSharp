namespace VaultSharp
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
        IGithubAuthBackend Github { get; }

        /// <summary>
        /// 
        /// </summary>
        IGoogleCloudAuthBackend GoogleCloud { get; }

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