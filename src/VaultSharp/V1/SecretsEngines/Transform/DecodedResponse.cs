using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transform
{
    /// <summary>
    /// Response for decoding.
    /// </summary>
    public class DecodedResponse : DecodedItem
    {
        /// <summary>
        /// Decoded items.
        /// </summary>
        [JsonProperty("batch_results")]
        public List<DecodedItem> DecodedItems { get; set; }
    }
}