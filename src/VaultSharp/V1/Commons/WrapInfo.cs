using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the wrapped information in Vault.
    /// </summary>
    public class WrapInfo
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("ttl")]
        public int TimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [JsonProperty("creation_time")]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the wrapped accessor.
        /// </summary>
        /// <value>
        /// The wrapped accessor.
        /// </value>
        [JsonProperty("accessor")]
        public string Accessor { get; set; }

        /// <summary>
        /// Gets or sets the creation path.
        /// </summary>
        /// <value>
        /// The creation path.
        /// </value>
        [JsonProperty("creation_path")]
        public string CreationPath { get; set; }
    }
}