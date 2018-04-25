using VaultSharp.AuthMethods;
using VaultSharp.Core;
using VaultSharp.SecretEngines;
using VaultSharp.SystemBackend;

namespace VaultSharp.V1
{
    internal class VaultClientV1 : IVaultClientV1
    {
        public VaultClientV1(Polymath polymath)
        { 
            SystemBackend = new SystemBackendProvider(polymath);
            AuthMethod = null;
            SecretEngine = null;
        }

        public ISecretEngine SecretEngine { get; }

        public IAuthBackend AuthMethod { get; }

        public ISystemBackend SystemBackend { get; }
    }
}
