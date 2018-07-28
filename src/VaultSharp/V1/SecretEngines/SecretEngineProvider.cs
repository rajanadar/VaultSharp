using System;
using VaultSharp.Core;
using VaultSharp.V1.SecretEngines.AWS;
using VaultSharp.V1.SecretEngines.Consul;
using VaultSharp.V1.SecretEngines.Cubbyhole;
using VaultSharp.V1.SecretEngines.Database;
using VaultSharp.V1.SecretEngines.Identity;
using VaultSharp.V1.SecretEngines.KeyValue;
using VaultSharp.V1.SecretEngines.PKI;
using VaultSharp.V1.SecretEngines.RabbitMQ;
using VaultSharp.V1.SecretEngines.SSH;
using VaultSharp.V1.SecretEngines.TOTP;
using VaultSharp.V1.SecretEngines.Transit;

namespace VaultSharp.V1.SecretEngines
{
    internal class SecretEngineProvider : ISecretEngine
    {
        private readonly Polymath _polymath;

        public SecretEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public IAWSSecretBackend AWS => throw new NotImplementedException();

        public IConsulSecretBackend Consul => throw new NotImplementedException();

        public ICubbyholeSecretBackend Cubbyhole => throw new NotImplementedException();

        public IDatabaseSecretBackend Database => throw new NotImplementedException();

        public IKeyValueSecretBackend KeyValue => throw new NotImplementedException();

        public IIdentitySecretBackend Identity => throw new NotImplementedException();

        public IPKISecretBackend PKI => throw new NotImplementedException();

        public IRabbitMQSecretBackend RabbitMQ => throw new NotImplementedException();

        public ISSHSecretBackend SSH => throw new NotImplementedException();

        public ITOTPSecretBackend TOTP => throw new NotImplementedException();

        public ITransitSecretBackend Transit => throw new NotImplementedException();
    }
}