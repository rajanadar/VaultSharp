namespace VaultSharp.Backends.Secret
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecretBackend
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
