using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Token
{
    internal class TokenAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly TokenAuthMethodInfo _tokenAuthInfo;
        private readonly Polymath _polymath;

        public TokenAuthMethodLoginProvider(TokenAuthMethodInfo tokenAuthInfo, Polymath polymath)
        {
            Checker.NotNull(tokenAuthInfo, "tokenAuthInfo");
            Checker.NotNull(tokenAuthInfo.VaultToken, "tokenAuthInfo.VaultToken");
            Checker.NotNull(polymath, "polymath");

            this._tokenAuthInfo = tokenAuthInfo;
            this._polymath = polymath;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            return await Task.FromResult(_tokenAuthInfo.VaultToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}