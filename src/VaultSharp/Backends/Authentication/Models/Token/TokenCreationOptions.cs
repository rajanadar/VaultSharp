using System;
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
        /// Gets or sets the name of the role.
        /// The token will be created against the specified role name; 
        /// this may override options set during this call.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [JsonIgnore]
        public string RoleName { get; set; }

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
        /// Gets or sets a value such that when true, the default policy will not be a part of this token's policy set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no default policy]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("no_default_policy")]
        public bool NoDefaultPolicy { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Set to false to disable the ability of the token to be renewed past its initial TTL. 
        /// Specifying true, or omitting this option, will allow the token to be renewable up to the system/mount maximum TTL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renewable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// DEPRECATED. Use <see cref="TimeToLive" />.
        /// </summary>
        /// <value>
        /// The lease time to live.
        /// </value>
        [JsonProperty("lease")]
        [Obsolete("Use TimeToLive instead")]
        public string Lease { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the lease time to live period of the token, provided as "1h", where hour is the largest suffix. 
        /// If not provided, the token is valid for the default lease TTL, or indefinitely if the root policy is used..
        /// </summary>
        /// <value>
        /// The lease time to live.
        /// </value>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// If set, the token will have an explicit max TTL set upon it. 
        /// This maximum token TTL cannot be changed later, and unlike with normal tokens, 
        /// updates to the system/mount max TTL value will have no effect at 
        /// renewal time -- the token will never be able to be renewed or used past the value set at issue time.
        /// </summary>
        /// <value>
        /// The explicit maximum time to live.
        /// </value>
        [JsonProperty("explicit_max_ttl")]
        public string ExplicitMaximumTimeToLive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCreationOptions"/> class.
        /// </summary>
        public TokenCreationOptions()
        {
            Renewable = true;
        }
    }
}