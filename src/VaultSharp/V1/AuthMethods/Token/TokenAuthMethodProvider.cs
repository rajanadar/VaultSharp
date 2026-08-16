using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.Token.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Token
{
    internal class TokenAuthMethodProvider : ITokenAuthMethod
    {
        private readonly Polymath _polymath;

        public TokenAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }

        public async Task<Secret<object>> CreateTokenAsync(CreateTokenRequest createTokenRequest)
        {
            var request = createTokenRequest ?? new CreateTokenRequest();

            var suffix = "create";

            if (request.CreateOrphan)
            {
                suffix = "create-orphan";
            }

            if (!string.IsNullOrWhiteSpace(request.RoleName))
            {
                suffix = suffix + "/" + request.RoleName.Trim('/');
            }

            return await _polymath.MakeVaultApiRequest<Secret<object>>("v1/auth/token/" + suffix, HttpMethod.Post, request).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateTokenRoleAsync(string roleName, CreateTokenRoleRequest createTokenRoleRequest)
        {
            Checker.NotNull(roleName, "roleName");
            var request = createTokenRoleRequest ?? new CreateTokenRoleRequest();
            await _polymath.MakeVaultApiRequest("v1/auth/token/roles/" + roleName.Trim('/'), HttpMethod.Post, request)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteTokenRoleAsync(string roleName)
        {
            Checker.NotNull(roleName, "roleName");
            await _polymath.MakeVaultApiRequest("v1/auth/token/roles/" + roleName.Trim('/'), HttpMethod.Delete)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListTokenRoles>> ListTokenRolesAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListTokenRoles>>("v1/auth/token/roles", _polymath.ListHttpMethod)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ClientTokenInfo>> LookupAsync(string clientToken)
        {
            Checker.NotNull(clientToken, nameof(clientToken));

            var requestData = new TokenRequest { Token = clientToken };
            return await _polymath.MakeVaultApiRequest<Secret<ClientTokenInfo>>("v1/auth/token/lookup", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CallingTokenInfo>> LookupSelfAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<CallingTokenInfo>>("v1/auth/token/lookup-self", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TokenRoleInfo>> ReadTokenRoleAsync(string roleName)
        {
            Checker.NotNull(roleName, "roleName");
            return await _polymath.MakeVaultApiRequest<Secret<TokenRoleInfo>>("v1/auth/token/roles/" + roleName.Trim('/'), HttpMethod.Get)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<AuthInfo> RenewSelfAsync(string increment = null)
        {
            var requestData = !string.IsNullOrWhiteSpace(increment) ? new IncrementRequest { Increment = increment } : null;

            var result = await _polymath.MakeVaultApiRequest<Secret<JsonObject>>("v1/auth/token/renew-self", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result.AuthInfo;
        }

        public async Task RevokeSelfAsync()
        {
            await _polymath.MakeVaultApiRequest<Secret<JsonObject>>("v1/auth/token/revoke-self", HttpMethod.Post).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}