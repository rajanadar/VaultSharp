using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The options set when requesting data r-eencryption (rewrap)
    /// </summary>
    public class RewrapRequestOptions : RewrapItem
    {
        /// <summary>
        /// Specifies the version of the key to use for the operation. If not set, uses the latest version.
        /// </summary>
        [JsonProperty(PropertyName = "key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        /// Specifies a list of items to be decrypted in a single batch.
        /// </summary>
        [JsonProperty(PropertyName = "batch_input")]
        public List<DecryptionItem> BatchedRewrapItems { get; set; }
    }
}