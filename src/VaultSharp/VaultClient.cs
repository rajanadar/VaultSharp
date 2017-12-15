namespace VaultSharp
{
    /// <summary>
    /// 
    /// </summary>
    public class VaultClient : IVaultClient
    {
        private VaultClientSettings vaultClientSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vaultClientSettings"></param>
        public VaultClient(VaultClientSettings vaultClientSettings)
        {
            this.vaultClientSettings = vaultClientSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        public IVaultClientV1 V1 => new VaultClientV1();

        /// <summary>
        /// 
        /// </summary>
        public VaultClientSettings Settings => vaultClientSettings;
    }
}
