using System;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Core;

namespace VaultSharp.V1.AuthMethods
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