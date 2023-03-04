using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the Encrypt Request Options.
    /// </summary>
    public class EncryptRequestOptions : EncryptionItem
    {
        /// <summary>
        /// [optional]
        /// Specifies a list of items to be encrypted in a single batch. 
        /// When this parameter is set, if the parameters 'plaintext', 'context' and 'nonce' are also set, they will be ignored.
        /// </summary>
        [JsonPropertyName("batch_input")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<EncryptionItem> BatchedEncryptionItems { get; set; }

        public EncryptRequestOptions()
        {
            this.KeyType = TransitKeyType.aes256_gcm96;
            this.PartialFailureResponseCode = (int)HttpStatusCode.BadRequest;
        }
    }
}