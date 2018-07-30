using System;
using VaultSharp.Core;
using VaultSharp.V1.SecretsEngines.AWS;
using VaultSharp.V1.SecretsEngines.Consul;
using VaultSharp.V1.SecretsEngines.Cubbyhole;
using VaultSharp.V1.SecretsEngines.Database;
using VaultSharp.V1.SecretsEngines.Identity;
using VaultSharp.V1.SecretsEngines.KeyValue;
using VaultSharp.V1.SecretsEngines.PKI;
using VaultSharp.V1.SecretsEngines.RabbitMQ;
using VaultSharp.V1.SecretsEngines.SSH;
using VaultSharp.V1.SecretsEngines.TOTP;
using VaultSharp.V1.SecretsEngines.Transit;

namespace VaultSharp.V1.SecretsEngines
{
    internal class SecretsEngineProvider : ISecretsEngine
    {
        private readonly Polymath _polymath;

        public SecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;

            Consul = new ConsulSecretsEngineProvider(polymath);
            KeyValue = new KeyValueSecretsEngineProvider(polymath);            
        }

        public IAWSSecretsEngine AWS => throw new NotImplementedException();

        public IConsulSecretsEngine Consul { get; }

        public ICubbyholeSecretsEngine Cubbyhole => throw new NotImplementedException();

        public IDatabaseSecretsEngine Database => throw new NotImplementedException();

        public IKeyValueSecretsEngine KeyValue { get; }

        public IIdentitySecretsEngine Identity => throw new NotImplementedException();

        public IPKISecretsEngine PKI => throw new NotImplementedException();

        public IRabbitMQSecretsEngine RabbitMQ => throw new NotImplementedException();

        public ISSHSecretsEngine SSH => throw new NotImplementedException();

        public ITOTPSecretsEngine TOTP => throw new NotImplementedException();

        public ITransitSecretsEngine Transit => throw new NotImplementedException();
    }
}