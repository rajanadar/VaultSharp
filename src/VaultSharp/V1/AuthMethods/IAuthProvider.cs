using System.Threading.Tasks;

namespace VaultSharp.V1.AuthMethods
{
    internal interface IAuthProvider
    {
        Task<string> GetVaultTokenAsync();
    }
}
