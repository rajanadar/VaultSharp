using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Infrastructure.Validation;

namespace VaultSharp.Backends.Authentication.Providers.Token
{
    internal class TokenAuthenticationProvider : IAuthenticationProvider
    {
        private readonly TokenAuthenticationInfo _tokenAuthenticationInfo;

        public TokenAuthenticationProvider(TokenAuthenticationInfo tokenAuthenticationInfo)
        {
            Checker.NotNull(tokenAuthenticationInfo, "tokenAuthenticationInfo");
            Checker.NotNull(tokenAuthenticationInfo.Token, "tokenAuthenticationInfo.Token");

            _tokenAuthenticationInfo = tokenAuthenticationInfo;
        }

        public async Task<string> GetTokenAsync()
        {
            return await Task.FromResult(_tokenAuthenticationInfo.Token);
        }
    }
}