using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.AppRole;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;

namespace VaultSharp.Backends.Authentication.Providers.AppRole
{
    internal class AppRoleAuthenticationProvider : IAuthenticationProvider
    {
        private readonly AppRoleAuthenticationInfo _appRoleAuthenticationInfo;
        private readonly IDataAccessManager _dataAccessManager;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public AppRoleAuthenticationProvider(AppRoleAuthenticationInfo appRoleAuthenticationInfo, IDataAccessManager dataAccessManager, bool continueAsyncTasksOnCapturedContext = false)
        {
            _appRoleAuthenticationInfo = appRoleAuthenticationInfo;
            _dataAccessManager = dataAccessManager;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestData = new Dictionary<string, object>
            {
                {"role_id", _appRoleAuthenticationInfo.RoleId}
            };

            if (!string.IsNullOrWhiteSpace(_appRoleAuthenticationInfo.SecretId))
            {
                requestData.Add("secret_id", _appRoleAuthenticationInfo.SecretId);
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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "auth/{0}/login", _appRoleAuthenticationInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}