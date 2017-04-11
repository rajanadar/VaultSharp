using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.AppId;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;

namespace VaultSharp.Backends.Authentication.Providers.AppId
{
    [Obsolete("The AppId Authentication backend in Vault is now deprecated with the addition " +
          "of the new AppRole backend. There are no plans to remove it, but we encourage " +
          "using AppRole whenever possible, as it offers enhanced functionality " +
          "and can accommodate many more types of authentication paradigms.")]
    internal class AppIdAuthenticationProvider : IAuthenticationProvider
    {
        private readonly AppIdAuthenticationInfo _appIdAuthenticationInfo;
        private readonly IDataAccessManager _dataAccessManager;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public AppIdAuthenticationProvider(AppIdAuthenticationInfo appIdAuthenticationInfo, IDataAccessManager dataAccessManager, bool continueAsyncTasksOnCapturedContext = false)
        {
            _appIdAuthenticationInfo = appIdAuthenticationInfo;
            _dataAccessManager = dataAccessManager;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestData = new
            {
                app_id = _appIdAuthenticationInfo.AppId,
                user_id = _appIdAuthenticationInfo.UserId
            };

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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "auth/{0}/login", _appIdAuthenticationInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}