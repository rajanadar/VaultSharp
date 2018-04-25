using System;
using VaultSharp.AuthMethods.Token;
using VaultSharp.Core;

namespace VaultSharp.AuthMethods
{
    internal static class AuthProviderFactory
    {
        public static IAuthProvider CreateAuthenticationProvider(IAuthInfo authInfo, Polymath polymath)
        {
            if (authInfo.BackendType == AuthBackendType.Token)
            {
                return new TokenAuthProvider(authInfo as TokenAuthInfo);
            }

            throw new NotSupportedException("The requested authentication backend type is not supported: " + authInfo.BackendType);
        }
    }
}