using System.Threading.Tasks;
using VaultSharp.Core;

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

        public Task<string> LoginAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}