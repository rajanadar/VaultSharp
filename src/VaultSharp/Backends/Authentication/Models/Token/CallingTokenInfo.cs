using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.Token
{
    /// <summary>
    /// Represents the information associated with the calling token.
    /// </summary>
    public class CallingTokenInfo : TokenAccessorInfo
    {
        /// <summary>
        /// Gets or sets the accessor.
        /// </summary>
        /// <value>
        /// The accessor.
        /// </value>
        [JsonProperty("accessor")]
        public string Accessor { get; set; }

        /// <summary>
        /// Gets or sets the explicit maximum time to live.
        /// </summary>
        /// <value>
        /// The explicit maximum time to live.
        /// </value>
        [JsonProperty("explicit_max_ttl")]
        public int ExplicitMaximumTimeToLive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CallingTokenInfo"/> is renewable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renewable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("renewable")]
        public bool Renewable { get; set; }
    }
}