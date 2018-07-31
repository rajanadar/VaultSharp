using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1;

namespace VaultSharp
{
    /// <summary>
    /// The concrete Vault client class.
    /// </summary>
    public class VaultClient : IVaultClient
    {
        private readonly Polymath polymath;
        private readonly IVaultClientV1 vaultClient1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vaultClientSettings"></param>
        public VaultClient(VaultClientSettings vaultClientSettings)
        {
            polymath = new Polymath(vaultClientSettings);
            vaultClient1 = new VaultClientV1(polymath);
        }

        /// <summary>
        /// Gets the V1 Client interface for Vault Api.
        /// </summary>
        public IVaultClientV1 V1 => vaultClient1;

        /// <summary>
        /// Gets the Vault Client Settings.
        /// </summary>
        public VaultClientSettings Settings => polymath.VaultClientSettings;
    }
}
