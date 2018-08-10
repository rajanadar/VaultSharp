using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Token
{
    internal class TokenAuthMethodLoginProvider : IAuthMethodLoginProvider
    {
        private readonly TokenAuthMethodInfo _tokenAuthInfo;

        public TokenAuthMethodLoginProvider(TokenAuthMethodInfo tokenAuthInfo)
        {
            Checker.NotNull(tokenAuthInfo, "tokenAuthInfo");
            Checker.NotNull(tokenAuthInfo.VaultToken, "tokenAuthInfo.VaultToken");

            this._tokenAuthInfo = tokenAuthInfo;
        }

        public async Task<string> GetVaultTokenAsync()
        {
            return await Task.FromResult(_tokenAuthInfo.VaultToken);
        }
    }
}