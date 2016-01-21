using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the rekey progress.
    /// </summary>
    public class RekeyProgress
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RekeyProgress"/> is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if complete; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// Gets or sets the new master keys. (possibly pgp encrypted)
        /// </summary>
        /// <value>
        /// The master keys.
        /// </value>
        [JsonProperty("keys")]
        public string[] MasterKeys { get; set; }
    }
}