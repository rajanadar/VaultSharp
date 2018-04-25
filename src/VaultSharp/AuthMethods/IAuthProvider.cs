using System.Threading.Tasks;

namespace VaultSharp.AuthMethods
{
    internal interface IAuthProvider
    {
        Task<string> GetVaultTokenAsync();
    }
}
