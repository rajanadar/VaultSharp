using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Consul
{
    /// <summary>
    /// Represents the Consul role definition with the policy and token information.
    /// </summary>
    public class ConsulRoleDefinition
    {
        /// <summary>
        /// <para>[required] for a <see cref="ConsulTokenType.client"/> token. otherwise [optional].</para>
        /// Gets or sets the base64 encoded Consul ACL policy.
        /// </summary>
        /// <value>
        /// The policy.
        /// </value>
        [JsonProperty("policy")]
        public string Base64EncodedPolicy { get; set; }

        /// <summary>
        /// Gets or sets the type of token to create using this role.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        [JsonProperty("token_type")]
        public ConsulTokenType TokenType { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the duration of the lease value provided as a string duration with time suffix. 
        /// Hour is the largest suffix..
        /// </summary>
        /// <value>
        /// The duration of the lease.
        /// </value>
        [JsonProperty("lease")]
        public string LeaseDuration { get; set; }
    }
}