using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the encryption response.
    /// </summary>
    public class EncryptionResponse : CipherTextData
    {
        /// <summary>
        /// Gets or sets the batch results.
        /// </summary>
        /// <value>
        /// The batch results.
        /// </value>
        [JsonPropertyName("batch_results", NullValueHandling = NullValueHandling.Ignore)]
        public List<CipherTextData> BatchedResults
        {
            get; set;
        }
    }
}