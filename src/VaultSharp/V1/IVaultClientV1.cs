using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.SecretEngines;
using VaultSharp.V1.SystemBackend;

namespace VaultSharp.V1
{
    public interface IVaultClientV1
    {
        ISecretEngine SecretEngine { get; }

        IAuthMethod AuthMethod { get; }

        ISystemBackend SystemBackend { get; }
    }
}
