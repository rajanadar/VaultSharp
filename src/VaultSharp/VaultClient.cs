using System;
using VaultSharp.Backends;
using VaultSharp.Core;
using VaultSharp.V1;

namespace VaultSharp
{
    /// <summary>
    /// 
    /// </summary>
    public class VaultClient : IVaultClient
    {
        private readonly BackendConnector backendConnector;
        private readonly IVaultClientV1 vaultClient1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vaultClientSettings"></param>
        public VaultClient(VaultClientSettings vaultClientSettings)
        {
            backendConnector = new BackendConnector(vaultClientSettings);
            vaultClient1 = new VaultClientV1(backendConnector);
        }

        /// <summary>
        /// 
        /// </summary>
        public IVaultClientV1 V1 => vaultClient1;

        /// <summary>
        /// 
        /// </summary>
        public VaultClientSettings Settings => backendConnector.VaultClientSettings;
    }
}
