using System.Threading.Tasks;

namespace VaultSharp.Backends.Authentication.Providers
{
    internal interface IAuthenticationProvider
    {
        Task<string> GetTokenAsync();
    }
}
