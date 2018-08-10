using System.Net.Http;
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

        public async Task<Secret<CallingTokenInfo>> LookupSelfAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<CallingTokenInfo>>("v1/auth/token/lookup-self", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<AuthInfo> RenewSelfAsync(string increment = null)
        {
            var requestData = !string.IsNullOrWhiteSpace(increment) ? new { increment = increment } : null;

            var result = await _polymath.MakeVaultApiRequest<Secret<dynamic>>("v1/auth/token/renew-self", HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result.AuthInfo;
        }

        public async Task RevokeSelfAsync()
        {
            await _polymath.MakeVaultApiRequest<Secret<dynamic>>("v1/auth/token/revoke-self", HttpMethod.Post).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}