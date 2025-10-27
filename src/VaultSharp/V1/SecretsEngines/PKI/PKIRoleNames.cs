using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class PKIRoleNames
    /// <summary>
    /// Vault Response Model containing list of role keys (role names)
    /// </summary>
    {
        /// <summary>
        /// Gets or sets list of certificate keys (serial numbers)
        /// </summary>
        /// <value>
        /// List of certificate keys (serial numbers)
        /// </value>
        [JsonPropertyName("keys")]
        public List<string> Keys { get; set; }
    }
}
