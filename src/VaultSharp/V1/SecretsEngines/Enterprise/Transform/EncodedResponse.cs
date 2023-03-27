using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// Response for encoding.
    /// </summary>
    public class EncodedResponse : EncodedItem
    {
        /// <summary>
        /// Encoded items.
        /// </summary>
        [JsonPropertyName("batch_results")]
        public List<EncodedItem> EncodedItems { get; set; }
    }
}