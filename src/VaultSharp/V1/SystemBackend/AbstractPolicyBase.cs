using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents an abstract Vault Policy entity.
    /// </summary>
    public abstract class AbstractPolicyBase
    {
        /// <summary>
        /// Gets or sets the name of the policy.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}