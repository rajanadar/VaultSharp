using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.LDAP;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;

namespace VaultSharp.Backends.Authentication.Providers.LDAP
{
    internal class LDAPAuthenticationProvider : IAuthenticationProvider
    {
        private readonly LDAPAuthenticationInfo _ldapAuthenticationInfo;
        private readonly IDataAccessManager _dataAccessManager;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public LDAPAuthenticationProvider(LDAPAuthenticationInfo ldapAuthenticationInfo, IDataAccessManager dataAccessManager, bool continueAsyncTasksOnCapturedContext = false)
        {
            _ldapAuthenticationInfo = ldapAuthenticationInfo;
            _dataAccessManager = dataAccessManager;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestData = new Dictionary<string, string>
            {
                { "password", _ldapAuthenticationInfo.Password }
            };

            if (!string.IsNullOrWhiteSpace(_ldapAuthenticationInfo.MultiFactorPasscode))
            {
                requestData.Add("passcode", _ldapAuthenticationInfo.MultiFactorPasscode);
            }

            if (!string.IsNullOrWhiteSpace(_ldapAuthenticationInfo.MultiFactorMethod))
            {
                requestData.Add("method", _ldapAuthenticationInfo.MultiFactorMethod);
            }

            var response =
                await
                    _dataAccessManager.MakeRequestAsync<Secret<Dictionary<string, object>>>(LoginResourcePath,
                        HttpMethod.Post, requestData).ConfigureAwait(_continueAsyncTasksOnCapturedContext);

            if (response != null && response.AuthorizationInfo != null && !string.IsNullOrWhiteSpace(response.AuthorizationInfo.ClientToken))
            {
                return response.AuthorizationInfo.ClientToken;
            }

            throw new Exception("The call to the authentication backend did not yield a client token. Please verify your credentials.");
        }

        private string LoginResourcePath
        {
            get
            {
                var endpoint = string.Format(CultureInfo.InvariantCulture, "auth/{0}/login/{1}", _ldapAuthenticationInfo.MountPoint.Trim('/'),
                    _ldapAuthenticationInfo.Username);
                return endpoint;
            }
        }
    }
}