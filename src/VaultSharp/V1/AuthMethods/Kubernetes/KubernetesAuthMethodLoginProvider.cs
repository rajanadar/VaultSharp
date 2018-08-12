using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Kubernetes
{
    internal class KubernetesAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly KubernetesAuthMethodInfo _kubernetesAuthMethodInfo;
        private readonly Polymath _polymath;

        public KubernetesAuthMethodLoginProvider(KubernetesAuthMethodInfo kubernetesAuthMethodInfo, Polymath polymath)
        {
            _kubernetesAuthMethodInfo = kubernetesAuthMethodInfo;
            _polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            // make an unauthenticated call to Vault, since this is the call to get the token. It shouldn't need a token.
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(LoginResourcePath, HttpMethod.Post, _kubernetesAuthMethodInfo, unauthenticated: true).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            _kubernetesAuthMethodInfo.ReturnedLoginAuthInfo = response?.AuthInfo;

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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "v1/auth/{0}/login", _kubernetesAuthMethodInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}