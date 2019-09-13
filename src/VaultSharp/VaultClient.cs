using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1;
using System;

namespace VaultSharp
{
    /// <summary>
    /// The concrete Vault client class.
    /// </summary>
    public class VaultClient : IVaultClient, IDisposable
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
        /// Gets the V1 Client interface for Vault Api.
        /// </summary>
        public IVaultClientV1 V1 { get; }

        /// <summary>
        /// Gets the Vault Client Settings.
        /// </summary>
        public VaultClientSettings Settings => polymath.VaultClientSettings;

        public void Dispose()
        {
            polymath.Dispose();
        }
    }
}
