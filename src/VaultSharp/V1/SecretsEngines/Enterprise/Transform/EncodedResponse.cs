using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("batch_results")]
        public List<EncodedItem> EncodedItems { get; set; }
    }
}