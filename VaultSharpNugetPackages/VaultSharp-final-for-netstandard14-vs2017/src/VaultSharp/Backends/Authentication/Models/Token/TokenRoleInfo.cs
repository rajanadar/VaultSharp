using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.Token
{
    /// <summary>
    /// Represents the information associated with a token role.
    /// </summary>
    public class TokenRoleInfo : TokenRoleBase
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the allowed policies.
        /// If set, tokens can be created with any subset of the policies in this list, 
        /// rather than the normal semantics of tokens being a subset of the calling token's policies. 
        /// If this and <see cref="DisallowedPolicies"/> are both set, only this option takes effect.
        /// </summary>
        /// <value>
        /// The allowed policies.
        /// </value>
        [JsonProperty("allowed_policies")]
        public List<string> AllowedPolicies { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the disallowed policies.
        /// If set, successful token creation via this role will require that no policies in 
        /// the given list are requested. If both this and <see cref="AllowedPolicies"/> are set, 
        /// this option has no effect.
        /// </summary>
        /// <value>
        /// The disallowed policies.
        /// </value>
        [JsonProperty("disallowed_policies")]
        public List<string> DisallowedPolicies { get; set; }
    }
}