namespace VaultSharp
{
    /// <summary>
    /// Provides an interface to interact with Vault as a client.
    /// This is the only entry point for consuming the Vault Client.
    /// </summary>
    public interface IVaultClient
    {
        /// <summary>
        /// 
        /// </summary>
        VaultClientSettings Settings { get; }

        /// <summary>
        /// 
        /// </summary>
        IVaultClientV1 V1 { get; }
    }
}

