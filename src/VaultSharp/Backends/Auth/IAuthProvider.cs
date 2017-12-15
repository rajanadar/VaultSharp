using System.Threading.Tasks;

namespace VaultSharp.Backends.Auth
{
    internal interface IAuthProvider
    {
        Task<string> GetVaultTokenAsync();
    }
}
