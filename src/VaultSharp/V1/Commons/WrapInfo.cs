using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonPropertyName("ttl")]
        public int TimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [JsonPropertyName("creation_time")]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the wrapped accessor.
        /// </summary>
        /// <value>
        /// The wrapped accessor.
        /// </value>
        [JsonPropertyName("accessor")]
        public string Accessor { get; set; }

        /// <summary>
        /// Gets or sets the creation path.
        /// </summary>
        /// <value>
        /// The creation path.
        /// </value>
        [JsonPropertyName("creation_path")]
        public string CreationPath { get; set; }
    }
}