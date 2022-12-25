using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the decryption response.
    /// </summary>
    public class DecryptionResponse : PlainTextData
    {
        /// <summary>
        /// Gets or sets the batch results.
        /// </summary>
        /// <value>
        /// The batch results.
        /// </value>
        [JsonProperty("batch_results", NullValueHandling = NullValueHandling.Ignore)]
        public List<PlainTextData> BatchedResults
        {
            get; set;
        }
    }
}