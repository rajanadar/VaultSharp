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
    /// <summary>
    /// The secrets engine interface.
    /// </summary>
    public interface ISecretsEngine
    {
        /// <summary>
        /// The ActiveDirectory Secrets Engine.
        /// </summary>
        IActiveDirectorySecretsEngine ActiveDirectory { get; }

        /// <summary>
        /// The AliCloud Secrets Engine.
        /// </summary>
        IAliCloudSecretsEngine AliCloud { get; }

        /// <summary>
        /// The AWS Secrets Engine.
        /// </summary>
        IAWSSecretsEngine AWS { get; }

        /// <summary>
        /// The Azure Secrets Engine.
        /// </summary>
        IAzureSecretsEngine Azure { get; }

        /// <summary>
        /// The Consul Secrets Engine.
        /// </summary>
        IConsulSecretsEngine Consul { get; }

        /// <summary>
        /// The Cubbyhole Secrets Engine.
        /// </summary>
        ICubbyholeSecretsEngine Cubbyhole { get; }

        /// <summary>
        /// The Database Secrets Engine.
        /// </summary>
        IDatabaseSecretsEngine Database { get; }

        /// <summary>
        /// The GoogleCloud Secrets Engine.
        /// </summary>
        IGoogleCloudSecretsEngine GoogleCloud { get; }

        /// <summary>
        /// The KeyValue Secrets Engine.
        /// </summary>
        IKeyValueSecretsEngine KeyValue { get; }

        /// <summary>
        /// The Identity Secrets Engine.
        /// </summary>
        IIdentitySecretsEngine Identity { get; }

        /// <summary>
        /// The Nomad Secrets Engine.
        /// </summary>
        INomadSecretsEngine Nomad { get; }

        /// <summary>
        /// The PKI Secrets Engine.
        /// </summary>
        IPKISecretsEngine PKI { get; }

        /// <summary>
        /// The RabbitMQ Secrets Engine.
        /// </summary>
        IRabbitMQSecretsEngine RabbitMQ { get; }

        /// <summary>
        /// The SSH Secrets Engine.
        /// </summary>
        ISSHSecretsEngine SSH { get; }

        /// <summary>
        /// The TOTP Secrets Engine.
        /// </summary>
        ITOTPSecretsEngine TOTP { get; }

        /// <summary>
        /// The Transit Secrets Engine.
        /// </summary>
        ITransitSecretsEngine Transit { get; }
    }
}
