using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("batch_results")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<PlainTextData> BatchedResults
        {
            get; set;
        }
    }
}