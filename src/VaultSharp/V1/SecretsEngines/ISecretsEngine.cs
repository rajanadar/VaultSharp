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
    /// <summary>
    /// 
    /// </summary>
    public interface ISecretsEngine
    {
        /// <summary>
        /// 
        /// </summary>
        IAWSSecretsEngine AWS { get; }

        /// <summary>
        /// 
        /// </summary>
        IConsulSecretsEngine Consul { get; }


        /// <summary>
        /// 
        /// </summary>
        ICubbyholeSecretsEngine Cubbyhole { get; }


        /// <summary>
        /// 
        /// </summary>
        IDatabaseSecretsEngine Database { get; }


        /// <summary>
        /// 
        /// </summary>
        IKeyValueSecretsEngine KeyValue { get; }


        /// <summary>
        /// 
        /// </summary>
        IIdentitySecretsEngine Identity { get; }


        /// <summary>
        /// 
        /// </summary>
        IPKISecretsEngine PKI { get; }


        /// <summary>
        /// 
        /// </summary>
        IRabbitMQSecretsEngine RabbitMQ { get; }


        /// <summary>
        /// 
        /// </summary>
        ISSHSecretsEngine SSH { get; }


        /// <summary>
        /// 
        /// </summary>
        ITOTPSecretsEngine TOTP { get; }


        /// <summary>
        /// 
        /// </summary>
        ITransitSecretsEngine Transit { get; }
    }
}
