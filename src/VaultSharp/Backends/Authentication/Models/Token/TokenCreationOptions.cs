using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.Token
{
    /// <summary>
    /// Represents the options to create a token.
    /// </summary>
    public class TokenCreationOptions : TokenInfo
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value, such that when true and set by a root caller, the token will not have the parent token of the caller. 
        /// This creates a token with no parent.
        /// Use this flag when you want to create an orphaned token when calling with a root token.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no parent]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("no_parent")]
        public bool NoParent { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether to create the token as an orphan.
        /// Use this flag when you're calling with a non-root token.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [create as orphan]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool CreateAsOrphan { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value such that when true, the default profile will not be a part of this token's policy set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no default profile]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("no_default_profile")]
        public bool NoDefaultProfile { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the lease time to live period of the token, provided as "1h", where hour is the largest suffix. 
        /// If not provided, the token is valid for the default lease TTL, or indefinitely if the root policy is used..
        /// </summary>
        /// <value>
        /// The lease time to live.
        /// </value>
        [JsonProperty("ttl")]
        public string LeaseTimeToLive { get; set; }
    }
}