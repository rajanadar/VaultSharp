using Newtonsoft.Json;

namespace VaultSharp.SystemBackend
{
    /// <summary>
    /// Represents a Vault ACL Policy entity.
    /// </summary>
    public class ACLPolicy : AbstractPolicyBase
    {
        /// <summary>
        /// Gets or sets the policy document. 
        /// This can be base64-encoded to avoid string escaping.
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        [JsonProperty("policy")]
        public string Policy { get; set; }
    }
}