using System.Collections.Generic;
using Newtonsoft.Json;
using VaultSharp.V1.SystemBackend.Enterprise;

namespace VaultSharp.V1.AuthMethods.Token.Models
{
    /// <summary>
    /// Represents the information associated with a token.
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the ID of the client token. Can only be specified by a root token. 
        /// Otherwise, the token ID is a randomly generated UUID.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a list of policies for the token. 
        /// This must be a subset of the policies belonging to the token making the request, unless root. 
        /// If not specified, defaults to all the policies of the calling token.
        /// </summary>
        /// <value>
        /// The policies.
        /// </value>
        [JsonProperty("policies")]
        public List<string> Policies { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a map of string to string valued metadata. 
        /// This is passed through to the audit backends.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [JsonProperty("meta")]
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the path used to create the token in the first place.
        /// </summary>
        /// <value>
        /// The created by path.
        /// </value>
        [JsonProperty("path")]
        public string CreatedByPath { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the display name of the token. 
        /// Defaults to "token".
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the maximum uses for the given token. 
        /// This can be used to create a one-time-token or limited use token. 
        /// Defaults to 0, which has no limit to number of uses.
        /// </summary>
        /// <value>
        /// The maximum usage count.
        /// </value>
        [JsonProperty("num_uses")]
        public int MaximumUsageCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating when this token was last renewed.
        /// </summary>
        /// <value>
        ///   The epoch of the last renewal time
        /// </value>
        [JsonProperty("last_renewal_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LastRenewalTime { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the expire time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("expire_time")]
        public string ExpireTime { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a list of identity policies for the token. 
        /// </summary>
        /// <value>
        /// The identity policies.
        /// </value>
        [JsonProperty("identity_policies")]
        public List<string> IdentityPolicies { get; set; }

        /// <summary>
        /// Gets or sets the issue time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("issue_time")]
        public string IssueTime { get; set; }
    }
}