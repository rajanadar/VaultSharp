using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.AppRole
{
    internal class AppRoleAuthenticationProvider : IAuthProvider
    {
        private readonly AppRoleAuthMethodInfo _appRoleAuthMethodInfo;
        private readonly Polymath _polymath;

        public AppRoleAuthenticationProvider(AppRoleAuthMethodInfo appRoleAuthenticationInfo, Polymath polymath)
        {
            _appRoleAuthMethodInfo = appRoleAuthenticationInfo;
            _polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            var requestData = new Dictionary<string, object>
            {
                {"role_id", _appRoleAuthMethodInfo.RoleId}
            };

            if (!string.IsNullOrWhiteSpace(_appRoleAuthMethodInfo.SecretId))
            {
                requestData.Add("secret_id", _appRoleAuthMethodInfo.SecretId);
            }

            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(LoginResourcePath, HttpMethod.Post, requestData, unauthenticated: true);

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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "v1/auth/{0}/login", _appRoleAuthMethodInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}