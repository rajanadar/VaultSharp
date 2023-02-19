using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the response for a request to calculate HMAC.
    /// </summary>
    /// <seealso cref="VaultSharp.V1.SecretsEngines.Transit.HmacBatchResponse" />
    public class HmacResponse : HmacBatchResponse
    {
        /// <summary>
        /// Gets or sets the list of results if multiple HMACs were requested.
        /// </summary>
        /// <value>The list of results.</value>
        [JsonPropertyName("batch_results", NullValueHandling = NullValueHandling.Ignore)]
        public List<HmacBatchResponse> BatchResults { get; set; }
    }

    public class HmacBatchResponse
    {
        /// <summary>
        /// Gets or sets the HMAC value returned by Vault.
        /// </summary>
        /// <value>The HMAC value returned by Vault.</value>
        [JsonPropertyName("hmac")]
        public string Hmac { get; set; }

        /// <summary>
        /// Gets or sets the error response from Vault when unable to calculate an HMAC.
        /// </summary>
        /// <value>The error response.</value>
        [JsonPropertyName("error", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorResponse { get; set; }
    }
}