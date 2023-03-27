using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
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
        [JsonPropertyName("policy")]
        public string Policy { get; set; }
    }
}