using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the data returned by Vault on a request to verify a signature or HMAC.
    /// </summary>
    public class VerifyResponse : VerifySingleResponse
    {
        /// <summary>
        /// Gets or sets a list of results if multiple verifications were requested at once.
        /// </summary>
        /// <value>The list of results.</value>

        [JsonProperty("batch_results", NullValueHandling = NullValueHandling.Ignore)]
        public List<VerifySingleResponse> BatchResults { get; set; }
    }

    public class VerifySingleResponse
    {
        /// <summary>
        /// Gets or sets a value indicating the verification result.
        /// </summary>
        /// <value><c>true</c> if the valid; otherwise, <c>false</c>.</value>
        [JsonProperty("valid")]
        public bool Valid { get; set; }
    }
}