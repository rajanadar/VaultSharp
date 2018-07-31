using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend;

namespace VaultSharp.V1
{
    /// <summary>
    /// The V1 interface for the Vault Api.
    /// </summary>
    public interface IVaultClientV1
    {
        /// <summary>
        /// The Secrets Engine interface.
        /// </summary>
        ISecretsEngine SecretsEngine { get; }

        /// <summary>
        /// The Auth Method interface.
        /// </summary>
        IAuthMethod AuthMethod { get; }

        /// <summary>
        /// The System Backend interface.
        /// </summary>
        ISystemBackend SystemBackend { get; }
    }
}
