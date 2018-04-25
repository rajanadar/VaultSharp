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
    /// <summary>
    /// 
    /// </summary>
    public interface ISecretEngine
    {
        /// <summary>
        /// 
        /// </summary>
        IAWSSecretBackend AWS { get; }

        /// <summary>
        /// 
        /// </summary>
        IConsulSecretBackend Consul { get; }


        /// <summary>
        /// 
        /// </summary>
        ICubbyholeSecretBackend Cubbyhole { get; }


        /// <summary>
        /// 
        /// </summary>
        IDatabaseSecretBackend Database { get; }


        /// <summary>
        /// 
        /// </summary>
        IKeyValueSecretBackend KeyValue { get; }


        /// <summary>
        /// 
        /// </summary>
        IIdentitySecretBackend Identity { get; }


        /// <summary>
        /// 
        /// </summary>
        IPKISecretBackend PKI { get; }


        /// <summary>
        /// 
        /// </summary>
        IRabbitMQSecretBackend RabbitMQ { get; }


        /// <summary>
        /// 
        /// </summary>
        ISSHSecretBackend SSH { get; }


        /// <summary>
        /// 
        /// </summary>
        ITOTPSecretBackend TOTP { get; }


        /// <summary>
        /// 
        /// </summary>
        ITransitSecretBackend Transit { get; }
    }
}
