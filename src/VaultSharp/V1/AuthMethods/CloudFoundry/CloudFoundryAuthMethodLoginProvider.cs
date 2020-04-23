using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.CloudFoundry.Signature;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.CloudFoundry
{
    internal class CloudFoundryAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly CloudFoundryAuthMethodInfo _cloudFoundryAuthMethodInfo;
        private readonly Polymath _polymath;        

        public CloudFoundryAuthMethodLoginProvider(CloudFoundryAuthMethodInfo cloudFoundryAuthMethodInfo, Polymath polymath)
        {
            _cloudFoundryAuthMethodInfo = cloudFoundryAuthMethodInfo;
            _polymath = polymath;

        }

        public async Task<string> GetVaultTokenAsync()
        {
            var signingTime = DateTime.UtcNow;
            var signature = CloudFoundrySignatureProvider.GetSignature(signingTime, _cloudFoundryAuthMethodInfo.CFInstanceCertContent, _cloudFoundryAuthMethodInfo.RoleName, _cloudFoundryAuthMethodInfo.CFInstanceKeyContent);

            var requestData = new
            {
                role = _cloudFoundryAuthMethodInfo.RoleName,
                cf_instance_cert = _cloudFoundryAuthMethodInfo.CFInstanceCertContent,
                signing_time = CloudFoundrySignatureProvider.GetFormattedSigningTime(signingTime),
                signature
            };
            
            // make an unauthenticated call to Vault, since this is the call to get the token. 
            // It shouldn't need a token.
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(LoginResourcePath, HttpMethod.Post, requestData, unauthenticated: true).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            
            _cloudFoundryAuthMethodInfo.ReturnedLoginAuthInfo = response?.AuthInfo;

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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "v1/auth/{0}/login", _cloudFoundryAuthMethodInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}
