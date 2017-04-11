using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.UsernamePassword;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;

namespace VaultSharp.Backends.Authentication.Providers.UsernamePassword
{
    internal class UsernamePasswordAuthenticationProvider : IAuthenticationProvider
    {
        private readonly UsernamePasswordAuthenticationInfo _userPassAuthenticationInfo;
        private readonly IDataAccessManager _dataAccessManager;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public UsernamePasswordAuthenticationProvider(UsernamePasswordAuthenticationInfo userPassAuthenticationInfo, IDataAccessManager dataAccessManager, bool continueAsyncTasksOnCapturedContext = false)
        {
            _userPassAuthenticationInfo = userPassAuthenticationInfo;
            _dataAccessManager = dataAccessManager;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestData = new Dictionary<string, string>
            {
                { "password", _userPassAuthenticationInfo.Password }
            };

            if (!string.IsNullOrWhiteSpace(_userPassAuthenticationInfo.MultiFactorPasscode))
            {
                requestData.Add("passcode", _userPassAuthenticationInfo.MultiFactorPasscode);
            }

            if (!string.IsNullOrWhiteSpace(_userPassAuthenticationInfo.MultiFactorMethod))
            {
                requestData.Add("method", _userPassAuthenticationInfo.MultiFactorMethod);
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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "auth/{0}/login/{1}", _userPassAuthenticationInfo.MountPoint.Trim('/'),
                    _userPassAuthenticationInfo.Username);
                return endpoint;
            }
        }
    }
}