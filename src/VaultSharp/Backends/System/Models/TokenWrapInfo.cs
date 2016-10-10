using System;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the token wrapped information in Vault.
    /// </summary>
    public class TokenWrapInfo
    {
        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [JsonProperty("creation_time")]
        public DateTimeOffset CreationTime { get; set; }
        /// <summary>
        /// Gets or sets the creation time to live.
        /// </summary>
        /// <value>
        /// The creation time to live.
        /// </value>
        [JsonProperty("creation_ttl")]
        public int CreationTimeToLive { get; set; }
    }
}