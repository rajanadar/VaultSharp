using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// Response for decoding.
    /// </summary>
    public class DecodedResponse : DecodedItem
    {
        /// <summary>
        /// Decoded items.
        /// </summary>
        [JsonPropertyName("batch_results")]
        public List<DecodedItem> DecodedItems { get; set; }
    }
}