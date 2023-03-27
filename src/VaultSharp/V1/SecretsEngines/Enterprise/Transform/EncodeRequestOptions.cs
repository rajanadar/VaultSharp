using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
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
        [JsonPropertyName("batch_input")]
        public List<EncodingItem> BatchItems { get; set; }
    }
}