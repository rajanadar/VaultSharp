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
            var customAuthInfo = await _customAuthMethodInfo.CustomAuthInfoAsyncDelegate().ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            _customAuthMethodInfo.ReturnedLoginAuthInfo = customAuthInfo;

            if (!string.IsNullOrWhiteSpace(customAuthInfo?.ClientToken))
            {
                return customAuthInfo.ClientToken;
            }

            throw new Exception("The call to the Custom Auth method did not yield a client token. Please verify your logic.");
        }
    }
}