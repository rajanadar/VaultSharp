using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The options set when requesting data re-encryption (rewrap)
    /// </summary>
    public class RewrapRequestOptions : RewrapItem
    {
        /// <summary>
        /// Specifies a list of items to be decrypted in a single batch.
        /// </summary>
        [JsonPropertyName("batch_input", NullValueHandling = NullValueHandling.Ignore)]
        public List<RewrapItem> BatchedRewrapItems { get; set; }
    }
}