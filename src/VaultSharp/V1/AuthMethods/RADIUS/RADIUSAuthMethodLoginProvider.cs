using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.RADIUS
{
    internal class RADIUSAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly RADIUSAuthMethodInfo _radiusAuthMethodInfo;
        private readonly Polymath _polymath;

        public RADIUSAuthMethodLoginProvider(RADIUSAuthMethodInfo radiusAuthMethodInfo, Polymath polymath)
        {
            _radiusAuthMethodInfo = radiusAuthMethodInfo;
            _polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            var requestData = new Dictionary<string, object>
            {
                {"password", _radiusAuthMethodInfo.Password }
            };

            // make an unauthenticated call to Vault, since this is the call to get the token. It shouldn't need a token.
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(LoginResourcePath, HttpMethod.Post, requestData, unauthenticated: true).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            _radiusAuthMethodInfo.ReturnedLoginAuthInfo = response?.AuthInfo;

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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "v1/auth/{0}/login/{1}", _radiusAuthMethodInfo.MountPoint.Trim('/'), _radiusAuthMethodInfo.Username);
                return endpoint;
            }
        }
    }
}