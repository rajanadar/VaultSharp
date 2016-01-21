using System;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.Custom;

namespace VaultSharp.Backends.Authentication.Providers.Custom
{
    internal class CustomAuthenticationProvider : IAuthenticationProvider
    {
        private readonly CustomAuthenticationInfo _customAuthenticationInfo;

        public CustomAuthenticationProvider(CustomAuthenticationInfo customAuthenticationInfo)
        {
            _customAuthenticationInfo = customAuthenticationInfo;
        }

        public async Task<string> GetTokenAsync()
        {
            var token = await _customAuthenticationInfo.AuthenticationTokenAsyncDelegate();

            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            throw new Exception("The call to the custom authentication delegate did not yield a client token. Please verify your delegate.");
        }
    }
}