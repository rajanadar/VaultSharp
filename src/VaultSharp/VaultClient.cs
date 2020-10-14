using VaultSharp.Core;
using VaultSharp.V1;

namespace VaultSharp
{
    /// <summary>
    /// The concrete Vault client class.
    /// </summary>
    public class VaultClient : IVaultClient
    {
        private readonly Polymath polymath;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="vaultClientSettings"></param>
        public VaultClient(VaultClientSettings vaultClientSettings)
        {
            polymath = new Polymath(vaultClientSettings);
            V1 = new VaultClientV1(polymath);
        }

        /// <summary>
        /// Clear current token.
        /// Next request will get new token.
        /// </summary>
        public void ResetToken()
        {
            polymath.ResetToken();
        }

        /// <summary>
        /// Gets the V1 Client interface for Vault Api.
        /// </summary>
        public IVaultClientV1 V1 { get; }

        /// <summary>
        /// Gets the Vault Client Settings.
        /// </summary>
        public VaultClientSettings Settings => polymath.VaultClientSettings;
    }
}
