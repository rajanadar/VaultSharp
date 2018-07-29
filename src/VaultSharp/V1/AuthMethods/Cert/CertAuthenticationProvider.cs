using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Cert
{
    internal class CertAuthenticationProvider : IAuthProvider
    {
        private readonly CertAuthMethodInfo _certAuthMethodInfo;
        private readonly Polymath _polymath;

        public CertAuthenticationProvider(CertAuthMethodInfo certAuthMethodInfo, Polymath polymath)
        {
            _certAuthMethodInfo = certAuthMethodInfo;
            _polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            // make an unauthenticated call to Vault, since this is the call to get the token. It shouldn't need a token.
            var response = await _polymath.MakeVaultApiRequest<Secret<dynamic>>(LoginResourcePath, HttpMethod.Post, unauthenticated: true);
            _certAuthMethodInfo.ReturnedLoginAuthInfo = response?.AuthInfo;

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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "v1/auth/{0}/login", _certAuthMethodInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}