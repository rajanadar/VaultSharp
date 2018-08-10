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
    }
}