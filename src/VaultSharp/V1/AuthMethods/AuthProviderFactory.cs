using System;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.AWS;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.Custom;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.LDAP;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace VaultSharp.V1.AuthMethods
{
    internal static class AuthProviderFactory
    {
        public static IAuthMethodLoginProvider CreateAuthenticationProvider(IAuthMethodInfo authInfo, Polymath polymath)
        {
            if (authInfo.AuthMethodType == AuthMethodType.AppRole)
            {
                return new AppRoleAuthMethodLoginProvider(authInfo as AppRoleAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.AWS)
            {
                return new AWSAuthMethodLoginProvider(authInfo as AbstractAWSAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.GitHub)
            {
                return new GitHubAuthMethodLoginProvider(authInfo as GitHubAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.LDAP)
            {
                return new LDAPAuthMethodLoginProvider(authInfo as LDAPAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Cert)
            {
                // we have attached the certificates to request elsewhere.
                return new CertAuthMethodLoginProvider(authInfo as CertAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Token)
            {
                return new TokenAuthMethodLoginProvider(authInfo as TokenAuthMethodInfo);
            }

            if (authInfo.AuthMethodType == AuthMethodType.UserPass)
            {
                return new UserPassAuthMethodLoginProvider(authInfo as UserPassAuthMethodInfo, polymath);
            }

            var customAuthMethodInfo = authInfo as CustomAuthMethodInfo;

            if (customAuthMethodInfo != null)
            {
                return new CustomAuthMethodLoginProvider(customAuthMethodInfo, polymath);
            }

            throw new NotSupportedException("The requested authentication backend type is not supported: " + authInfo.AuthMethodType);
        }
    }
}