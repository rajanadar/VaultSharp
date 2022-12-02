using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.RabbitMQ
{
    public class RabbitMQLease
    {

        /// <summary>
        /// Specifies the lease ttl provided in seconds.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("ttl")]
        public int TimeToLive { get; set; }

        /// <summary>
        /// Specifies the maximum ttl provided in seconds.
        /// </summary>
        [JsonProperty("max_ttl")]
        public int MaximumTimeToLive { get; set; }
    }
}
