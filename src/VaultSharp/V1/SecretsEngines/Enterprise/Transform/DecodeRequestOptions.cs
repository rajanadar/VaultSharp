using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// Represents the Decode Request Options.
    /// </summary>
    public class DecodeRequestOptions : DecodingItem
    {
        /// <summary>
        /// Specifies the transformation within the role that should be used for this decode operation. 
        /// If a single transformation exists for role, this parameter may be skipped and will be inferred. 
        /// If multiple transformations exist, one must be specified.
        /// </summary>
        [JsonProperty("batch_input")]
        public List<DecodingItem> BatchItems { get; set; }
    }
}