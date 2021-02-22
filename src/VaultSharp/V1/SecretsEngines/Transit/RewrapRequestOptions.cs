using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "batch_input")]
        public List<RewrapItem> BatchedRewrapItems { get; set; }
    }
}