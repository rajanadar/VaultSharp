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
            var customAuthMethodInfo = await _customAuthMethodInfo.CustomAuthMethodInfoAsyncDelegate().ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            if (!string.IsNullOrWhiteSpace(customAuthMethodInfo.ReturnedLoginAuthInfo?.ClientToken))
            {
                return customAuthMethodInfo.ReturnedLoginAuthInfo?.ClientToken;
            }

            throw new Exception("The call to the Custom Auth method did not yield a client token. Please verify your logic.");
        }
    }
}