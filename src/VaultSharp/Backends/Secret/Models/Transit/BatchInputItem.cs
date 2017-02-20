using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Transit
{
    /// <summary>
    /// The Batch Input item.
    /// </summary>
    public class BatchInputItem
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        [JsonProperty("context")]
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the plain text.
        /// </summary>
        /// <value>
        /// The plain text.
        /// </value>
        [JsonProperty("plaintext")]
        public string PlainText { get; set; }
    }
}