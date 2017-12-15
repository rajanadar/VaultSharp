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
        /// 
        /// </summary>
        public IVaultClientV1 V1 => vaultClient1;

        /// <summary>
        /// 
        /// </summary>
        public VaultClientSettings Settings => polymath.VaultClientSettings;
    }
}
