using Newtonsoft.Json;

namespace VaultSharp.SystemBackend.Enterprise
{
    /// <summary>
    /// Represents a Vault GP Policy entity.
    /// </summary>
    public abstract class AbstractGPPolicyBase : AbstractPolicyBase
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

        /// <summary>
        /// Gets or sets the enforcement level to use. 
        /// This must be one of advisory, soft-mandatory, or hard-mandatory.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [JsonProperty("enforcement_level")]
        public EnforcementLevel EnforcementLevel { get; set; }
    }
}