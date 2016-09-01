using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the progress of the root token generation attempt.
    /// </summary>
    public class RootTokenGenerationProgress : RootTokenGenerationStatus
    {
        /// <summary>
        /// Gets or sets the encoded root token if the attempt is complete.
        /// </summary>
        /// <value>
        /// The encoded root token.
        /// </value>
        [JsonProperty("encoded_root_token")]
        public string EncodedRootToken { get; set; }
    }
}