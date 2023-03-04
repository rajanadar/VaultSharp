using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the response returned by Vault on a request to sign a data string.
    /// </summary>
    public class SigningResponse : SigningBatchResponse
    {
        /// <summary>
        /// Gets or sets a list of results if multiple signatures were requested at once.
        /// </summary>
        /// <value>The list of results.</value>
        [JsonPropertyName("batch_results")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<SigningBatchResponse> BatchResults { get; set; }
    }

    public class SigningBatchResponse
    {
        /// <summary>
        /// Gets or sets the error message returned if unable to sign the input data string.
        /// </summary>
        /// <value>The error.</value>
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; set; }


        [JsonPropertyName("key_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? KeyVersion { get; set; }

        /// <summary>
        /// Gets or sets the derived public key used for the signature, if requested.
        /// </summary>
        /// <value>The derived public key.</value>
        [JsonPropertyName("publickey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the signature for the input data string.
        /// </summary>
        /// <value>The signature.</value>
        [JsonPropertyName("signature")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Signature { get; set; }
    }
}