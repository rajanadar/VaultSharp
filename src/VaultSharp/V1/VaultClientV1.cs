using VaultSharp.Core;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend;

namespace VaultSharp.V1
{
    internal class VaultClientV1 : IVaultClientV1
    {
        public VaultClientV1(Polymath polymath)
        { 
            SystemBackend = new SystemBackendProvider(polymath);
            AuthMethod = new AuthMethodProvider(polymath);
            SecretsEngine = new SecretsEngineProvider(polymath);
        }

        public ISecretsEngine SecretsEngine { get; }

        public IAuthMethod AuthMethod { get; }

        public ISystemBackend SystemBackend { get; }
    }
}
