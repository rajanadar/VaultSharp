using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the token wrap data.
    /// </summary>
    public class TokenWrapData
    {
        /// <summary>
        /// Gets or sets the creation path.
        /// </summary>
        /// <value>
        /// The creation path.
        /// </value>
        [JsonPropertyName("creation_path")]
        public string CreationPath { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [JsonPropertyName("creation_time")]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time to live.
        /// </summary>
        /// <value>
        /// The creation time to live.
        /// </value>
        [JsonPropertyName("creation_ttl")]
        public int CreationTimeToLive { get; set; }
    }
}