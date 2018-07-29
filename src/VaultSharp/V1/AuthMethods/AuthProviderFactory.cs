using System;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.Custom;
using VaultSharp.V1.AuthMethods.LDAP;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace VaultSharp.V1.AuthMethods
{
    internal static class AuthProviderFactory
    {
        public static IAuthProvider CreateAuthenticationProvider(IAuthMethodInfo authInfo, Polymath polymath)
        {
            if (authInfo.AuthMethodType == AuthMethodType.AppRole)
            {
                return new AppRoleAuthenticationProvider(authInfo as AppRoleAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.LDAP)
            {
                return new LDAPAuthenticationProvider(authInfo as LDAPAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Cert)
            {
                // we have attached the certificates to request elsewhere.
                return new CertAuthenticationProvider(authInfo as CertAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Token)
            {
                return new TokenAuthProvider(authInfo as TokenAuthMethodInfo);
            }

            if (authInfo.AuthMethodType == AuthMethodType.UserPass)
            {
                return new UserPassAuthenticationProvider(authInfo as UserPassAuthMethodInfo, polymath);
            }

            var customAuthMethodInfo = authInfo as CustomAuthMethodInfo;

            if (customAuthMethodInfo != null)
            {
                return new CustomAuthenticationProvider(customAuthMethodInfo, polymath);
            }

            throw new NotSupportedException("The requested authentication backend type is not supported: " + authInfo.AuthMethodType);
        }
    }
}