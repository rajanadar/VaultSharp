using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the Decrypt Request Options.
    /// </summary>
    public class DecryptRequestOptions : DecryptionItem
    {
        /// <summary>
        /// [optional]
        /// Specifies a list of items to be decrypted in a single batch. 
        /// When this parameter is set, if the parameters 'ciphertext', 'context' and 'nonce' are also set, they will be ignored.
        /// </summary>
        [JsonPropertyName("batch_input")]
        public List<DecryptionItem> BatchedDecryptionItems { get; set; }
    }
}