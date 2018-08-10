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
            System = new SystemBackendProvider(polymath);
            Auth = new AuthMethodProvider(polymath);
            Secrets = new SecretsEngineProvider(polymath);
        }

        public ISecretsEngine Secrets { get; }

        public IAuthMethod Auth { get; }

        public ISystemBackend System { get; }
    }
}
