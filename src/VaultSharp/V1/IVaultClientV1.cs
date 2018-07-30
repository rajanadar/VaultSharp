using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend;

namespace VaultSharp.V1
{
    public interface IVaultClientV1
    {
        ISecretsEngine SecretsEngine { get; }

        IAuthMethod AuthMethod { get; }

        ISystemBackend SystemBackend { get; }
    }
}
