using System;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Custom
{
    internal class CustomAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly CustomAuthMethodInfo _customAuthMethodInfo;
        private readonly Polymath _polymath;

        public CustomAuthMethodLoginProvider(CustomAuthMethodInfo customAuthMethodInfo, Polymath polymath)
        {
            _customAuthMethodInfo = customAuthMethodInfo;
            _polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            string token;
            if (_customAuthMethodInfo.AuthenticationInfoAsyncDelegate != null)
            {
                var authInfo = await _customAuthMethodInfo.AuthenticationInfoAsyncDelegate()
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
                _customAuthMethodInfo.ReturnedLoginAuthInfo = authInfo;
                token = authInfo?.ClientToken;
            }
            else
            {
                token = await _customAuthMethodInfo.AuthenticationTokenAsyncDelegate()
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            throw new Exception(
                "The call to the Custom Auth method did not yield a client token. Please verify your credentials.");
        }
    }
}