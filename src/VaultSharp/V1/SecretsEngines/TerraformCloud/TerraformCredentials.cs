using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines
{
    /// <summary>
    /// Represents Terraform credentials
    /// </summary>
    public class TerraformCredentials
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token id.
        /// </summary>
        /// <value>
        /// The token id.
        /// </value>
        [JsonProperty("token_id")]
        public string TokenId { get; set; }
    }
}