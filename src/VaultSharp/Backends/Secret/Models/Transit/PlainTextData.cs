using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Transit
{
    /// <summary>
    /// Represents the Plain text data.
    /// </summary>
    public class PlainTextData
    {
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