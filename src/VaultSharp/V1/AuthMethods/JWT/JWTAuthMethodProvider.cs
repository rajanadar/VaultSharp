using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.JWT.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.JWT
{
    internal class JWTAuthMethodProvider : IJWTAuthMethod
    {
        private readonly Polymath _polymath;

        public JWTAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }

        public async Task<Secret<OIDCAuthURLInfo>> GetOIDCAuthURLAsync(string redirectUri, string roleName = null, string clientNonce = null, string mountPoint = AuthMethodDefaultPaths.JWT)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(redirectUri, "redirectUri");

            var requestData = new Dictionary<string, string>
            {
                { "redirect_uri", redirectUri }
            };

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                requestData.Add("role", roleName);
            }

            if (!string.IsNullOrWhiteSpace(clientNonce))
            {
                requestData.Add("client_nonce", clientNonce);
            }

            return await _polymath.MakeVaultApiRequest<Secret<OIDCAuthURLInfo>>("v1/auth/" + mountPoint.Trim('/') + "/oidc/auth_url", HttpMethod.Post, requestData: requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AuthInfo>> DoOIDCCallbackAsync(string state, string nonce, string code, string clientNonce = null, string mountPoint = AuthMethodDefaultPaths.JWT)
        {
            Checker.NotNull(state, "state");
            Checker.NotNull(nonce, "nonce");
            Checker.NotNull(code, "code");

            Checker.NotNull(mountPoint, "mountPoint");

            var queryStrings = new List<string>
            {
                "state=" + state,
                "nonce=" + nonce,
                "code=" + code
            };

            if (!string.IsNullOrWhiteSpace(clientNonce))
            {
                queryStrings.Add("client_nonce=" + clientNonce);
            }

            var queryString = "?" + string.Join("&", queryStrings);

            return await _polymath.MakeVaultApiRequest<Secret<AuthInfo>>("v1/auth/" + mountPoint.Trim('/') + "/oidc/callback" + queryString, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}