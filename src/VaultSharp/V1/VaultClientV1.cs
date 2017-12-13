using System;

namespace VaultSharp
{
    public class VaultClientV1 : IVaultClientV1
    {
        public ISecretBackend Secret => throw new NotImplementedException();

        public IAuthBackend Auth => throw new NotImplementedException();

        public ISystemBackend System => throw new NotImplementedException();
    }
}
