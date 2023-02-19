using System.Text.Json.Serialization;

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
        [JsonPropertyName("ttl")]
        public int TimeToLive { get; set; }

        /// <summary>
        /// Specifies the maximum ttl provided in seconds.
        /// </summary>
        [JsonPropertyName("max_ttl")]
        public int MaximumTimeToLive { get; set; }
    }
}
