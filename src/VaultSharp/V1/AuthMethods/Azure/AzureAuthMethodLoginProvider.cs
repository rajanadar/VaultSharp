using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Azure
{
    internal class AzureAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly AzureAuthMethodInfo _azureAuthMethodInfo;
        private readonly Polymath _polymath;

        public AzureAuthMethodLoginProvider(AzureAuthMethodInfo azureAuthenticationInfo, Polymath polymath)
        {
            _azureAuthMethodInfo = azureAuthenticationInfo;
            _polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            // make an unauthenticated call to Vault, since this is the call to get the token. It shouldn't need a token.
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(LoginResourcePath, HttpMethod.Post, _azureAuthMethodInfo, unauthenticated: true).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            _azureAuthMethodInfo.ReturnedLoginAuthInfo = response?.AuthInfo;

            if (response?.AuthInfo != null && !string.IsNullOrWhiteSpace(response.AuthInfo.ClientToken))
            {
                return response.AuthInfo.ClientToken;
            }

            throw new Exception("The call to the Vault authentication method backend did not yield a client token. Please verify your credentials.");
        }

        private string LoginResourcePath
        {
            get
            {
                var endpoint = string.Format(CultureInfo.InvariantCulture, "v1/auth/{0}/login", _azureAuthMethodInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}