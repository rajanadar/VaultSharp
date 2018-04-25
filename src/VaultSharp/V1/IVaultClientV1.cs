using VaultSharp.AuthMethods;
using VaultSharp.SecretEngines;
using VaultSharp.SystemBackend;

namespace VaultSharp.V1
{
    public interface IVaultClientV1
    {
        ISecretEngine SecretEngine { get; }

        IAuthBackend AuthMethod { get; }

        ISystemBackend SystemBackend { get; }
    }
}
