using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.AuthMethods.Token
{
    internal class TokenAuthProvider : IAuthProvider
    {
        private readonly TokenAuthInfo _tokenAuthInfo;

        public TokenAuthProvider(TokenAuthInfo tokenAuthInfo)
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