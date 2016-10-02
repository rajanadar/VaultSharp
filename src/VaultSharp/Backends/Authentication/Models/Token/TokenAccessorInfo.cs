using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.Token
{
    /// <summary>
    /// Represents the information associated with a token accessor.
    /// </summary>
    public class TokenAccessorInfo : TokenInfo
    {
        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [JsonProperty("creation_time")]
        public int CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time to live.
        /// </summary>
        /// <value>
        /// The creation time to live.
        /// </value>
        [JsonProperty("creation_ttl")]
        public int CreationTimeToLive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TokenAccessorInfo"/> is orphan.
        /// </summary>
        /// <value>
        ///   <c>true</c> if orphan; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("orphan")]
        public bool Orphan { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>
        /// The time to live.
        /// </value>
        [JsonProperty("ttl")]
        public int TimeToLive { get; set; }
    }
}