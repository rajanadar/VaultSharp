using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transform
{
    /// <summary>
    /// Represents the Encode Request Options.
    /// </summary>
    public class EncodeRequestOptions : EncodingItem
    {
        /// <summary>
        /// Specifies a list of items to be encoded in a single batch. 
        /// When this parameter is set, the 'value', 'transformation' and 'tweak' parameters are ignored. 
        /// Instead, the aforementioned parameters should be provided within each object in the list.
        /// </summary>
        [JsonProperty("batch_input")]
        public List<EncodingItem> BatchItems { get; set; }
    }
}