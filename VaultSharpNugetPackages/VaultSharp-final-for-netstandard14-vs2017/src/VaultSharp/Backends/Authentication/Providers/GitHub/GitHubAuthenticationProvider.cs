using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.GitHub;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;

namespace VaultSharp.Backends.Authentication.Providers.GitHub
{
    internal class GitHubAuthenticationProvider : IAuthenticationProvider
    {
        private readonly GitHubAuthenticationInfo _gitHubAuthenticationInfo;
        private readonly IDataAccessManager _dataAccessManager;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public GitHubAuthenticationProvider(GitHubAuthenticationInfo gitHubAuthenticationInfo, IDataAccessManager dataAccessManager, bool continueAsyncTasksOnCapturedContext = false)
        {
            _gitHubAuthenticationInfo = gitHubAuthenticationInfo;
            _dataAccessManager = dataAccessManager;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestData = new
            {
                token = _gitHubAuthenticationInfo.PersonalAccessToken
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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "auth/{0}/login", _gitHubAuthenticationInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}