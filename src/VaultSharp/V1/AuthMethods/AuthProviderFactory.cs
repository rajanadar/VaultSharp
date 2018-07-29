using System;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;

namespace VaultSharp.V1.AuthMethods
{
    internal static class AuthProviderFactory
    {
        public static IAuthProvider CreateAuthenticationProvider(IAuthMethodInfo authInfo, Polymath polymath)
        {
            if (authInfo.AuthMethodType == AuthMethodType.AppRole)
            {
                return new UserPassAuthenticationProvider(authInfo as AppRoleAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Token)
            {
                return new TokenAuthProvider(authInfo as TokenAuthMethodInfo);
            }

            throw new NotSupportedException("The requested authentication backend type is not supported: " + authInfo.AuthMethodType);
        }
    }
}