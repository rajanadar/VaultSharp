using System;
using VaultSharp.Backends.Auth.Token;
using VaultSharp.Core;

namespace VaultSharp.Backends.Auth
{
    internal static class AuthProviderFactory
    {
        public static IAuthProvider CreateAuthenticationProvider(IAuthInfo authInfo, BackendConnector backendConnector)
        {
            if (authInfo.BackendType == AuthBackendType.Token)
            {
                return new TokenAuthProvider(authInfo as TokenAuthInfo);
            }

            throw new NotSupportedException("The requested authentication backend type is not supported: " + authInfo.BackendType);
        }
    }
}