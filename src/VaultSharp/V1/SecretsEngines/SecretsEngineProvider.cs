using System;
using VaultSharp.Core;
using VaultSharp.V1.SecretsEngines.ActiveDirectory;
using VaultSharp.V1.SecretsEngines.AliCloud;
using VaultSharp.V1.SecretsEngines.AWS;
using VaultSharp.V1.SecretsEngines.Azure;
using VaultSharp.V1.SecretsEngines.Consul;
using VaultSharp.V1.SecretsEngines.Cubbyhole;
using VaultSharp.V1.SecretsEngines.Database;
using VaultSharp.V1.SecretsEngines.GoogleCloud;
using VaultSharp.V1.SecretsEngines.Identity;
using VaultSharp.V1.SecretsEngines.KeyValue;
using VaultSharp.V1.SecretsEngines.Nomad;
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

            ActiveDirectory = new ActiveDirectorySecretsEngineProvider(polymath);
            AliCloud = new AliCloudSecretsEngineProvider(polymath);
            AWS = new AWSSecretsEngineProvider(polymath);
            Azure = new AzureSecretsEngineProvider(polymath);
            Consul = new ConsulSecretsEngineProvider(polymath);
            Cubbyhole = new CubbyholeSecretsEngineProvider(polymath);
            Database = new DatabaseSecretsEngineProvider(polymath);
            GoogleCloud = new GoogleCloudSecretsEngineProvider(polymath);
            KeyValue = new KeyValueSecretsEngineProvider(polymath);
            Nomad = new NomadSecretsEngineProvider(polymath);
            PKI = new PKISecretsEngineProvider(polymath);
            RabbitMQ = new RabbitMQSecretsEngineProvider(polymath);
            SSH = new SSHSecretsEngineProvider(polymath);
            TOTP = new TOTPSecretsEngineProvider(polymath);
            Transit = new TransitSecretsEngineProvider(polymath);
        }

        public IActiveDirectorySecretsEngine ActiveDirectory { get; }

        public IAliCloudSecretsEngine AliCloud { get; }

        public IAWSSecretsEngine AWS { get; }

        public IAzureSecretsEngine Azure { get; }

        public IConsulSecretsEngine Consul { get; }

        public ICubbyholeSecretsEngine Cubbyhole { get; }

        public IDatabaseSecretsEngine Database { get; }

        public IGoogleCloudSecretsEngine GoogleCloud { get; }

        public IKeyValueSecretsEngine KeyValue { get; }

        public IIdentitySecretsEngine Identity => throw new NotImplementedException();

        public INomadSecretsEngine Nomad { get; }

        public IPKISecretsEngine PKI { get; }

        public IRabbitMQSecretsEngine RabbitMQ { get; }

        public ISSHSecretsEngine SSH { get; }

        public ITOTPSecretsEngine TOTP { get; }

        public ITransitSecretsEngine Transit { get; }
    }
}