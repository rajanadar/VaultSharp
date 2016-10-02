using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.AwsEc2;
using VaultSharp.Backends.System.Models;
using VaultSharp.DataAccess;

namespace VaultSharp.Backends.Authentication.Providers.AwsEc2
{
    internal class AwsEc2AuthenticationProvider : IAuthenticationProvider
    {
        private readonly AwsEc2AuthenticationInfo _awsEc2AuthenticationInfo;
        private readonly IDataAccessManager _dataAccessManager;
        private readonly bool _continueAsyncTasksOnCapturedContext;

        public AwsEc2AuthenticationProvider(AwsEc2AuthenticationInfo awsEc2AuthenticationInfo, IDataAccessManager dataAccessManager, bool continueAsyncTasksOnCapturedContext = false)
        {
            _awsEc2AuthenticationInfo = awsEc2AuthenticationInfo;
            _dataAccessManager = dataAccessManager;
            _continueAsyncTasksOnCapturedContext = continueAsyncTasksOnCapturedContext;
        }

        public async Task<string> GetTokenAsync()
        {
            var requestData = new Dictionary<string, object>
            {
                {"pkcs7", _awsEc2AuthenticationInfo.Pkcs7}
            };

            if (!string.IsNullOrWhiteSpace(_awsEc2AuthenticationInfo.Nonce))
            {
                requestData.Add("nonce", _awsEc2AuthenticationInfo.Nonce);
            }

            if (!string.IsNullOrWhiteSpace(_awsEc2AuthenticationInfo.RoleName))
            {
                requestData.Add("role", _awsEc2AuthenticationInfo.RoleName);
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
                var endpoint = string.Format(CultureInfo.InvariantCulture, "auth/{0}/login", _awsEc2AuthenticationInfo.MountPoint.Trim('/'));
                return endpoint;
            }
        }
    }
}