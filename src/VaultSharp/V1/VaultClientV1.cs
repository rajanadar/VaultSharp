using System;
using VaultSharp.Backends;
using VaultSharp.Backends.Auth;
using VaultSharp.Backends.Secret;
using VaultSharp.Backends.System;

namespace VaultSharp
{
    internal class VaultClientV1 : IVaultClientV1
    {
        private ISecretBackend secretBackend;
        private IAuthBackend authBackend;
        private ISystemBackend systemBackend;

        public VaultClientV1(BackendConnector backendConnector)
        { 
            this.systemBackend = new SystemBackend(backendConnector);
        }

        public ISecretBackend Secret => secretBackend;

        public IAuthBackend Auth => authBackend;

        public ISystemBackend System => systemBackend;
    }
}
