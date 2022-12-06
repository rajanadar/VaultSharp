using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.RabbitMQ
{
    public class RabbitMQRole
    {
        /// <summary>
        /// Specifies a comma-separated RabbitMQ management tags
        /// </summary>
        /// <value>
        /// The tags
        /// </value>
        [JsonProperty("tags")] 
        public string Tags { get; set; }

        /// <summary>
        /// Specifies a map of virtual hosts to permissions. 
        /// This can be base64-encoded to avoid string escaping.
        /// See https://developer.hashicorp.com/vault/api-docs/secret/rabbitmq#create-role for examples
        /// </summary>
        /// <value>
        /// The virtual hosts role.
        /// </value>
        [JsonProperty("vhosts")]
        public string VHosts { get; set; }

        /// <summary>
        /// Specifies a map of virtual hosts and exchanges to topic permissions. This option requires RabbitMQ 3.7.0 or later. 
        /// This can be base64-encoded to avoid string escaping.
        /// See https://developer.hashicorp.com/vault/api-docs/secret/rabbitmq#create-role for examples
        /// </summary>
        /// <value>
        /// The virtual hosts topics.
        /// </value>
        [JsonProperty("vhost_topics")]
        public string VHostTopics { get; set; }
    }
}
