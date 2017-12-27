using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Enterprise
{
    /// <summary>
    /// Represents a Vault EGP Policy entity.
    /// </summary>
    public class EGPPolicy : AbstractGPPolicyBase
    {
        /// <summary>
        /// Gets or sets the paths.
        /// </summary>
        /// <value>
        /// The paths.
        /// </value>
        [JsonProperty("paths")]
        public IEnumerable<string> Paths { get; set; }
    }
}