using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents a Vault Policy entity.
    /// </summary>
    public class Policy : AbstractPolicyBase
    {
        /// <summary>
        /// Gets or sets the rules for the policy.
        /// <para>
        /// These rules are in a raw, HCL (Hashicorp Configuration Language) or JSON format.
        /// So write to them as a single raw string value from an HCL or JSON packet.
        /// And read them as a single raw string value and then parse them for HCL or JSON.
        /// </para>
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        [JsonProperty("rules")]
        public string Rules { get; set; }
    }
}