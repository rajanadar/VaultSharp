using System;
using VaultSharp.Backends;
using VaultSharp.Backends.Auth;
using VaultSharp.Backends.Secret;
using VaultSharp.Backends.System;
using VaultSharp.Core;

namespace VaultSharp.V1
{
    internal class VaultClientV1 : IVaultClientV1
    {
        private ISecretBackend secretBackend;
        private IAuthBackend authBackend;
        private ISystemBackend systemBackend;

        public VaultClientV1(Polymath polymath)
        { 
            this.systemBackend = new SystemBackendProvider(polymath);
        }

        public ISecretBackend Secret => secretBackend;

        public IAuthBackend Auth => authBackend;

        public ISystemBackend System => systemBackend;
    }
}
