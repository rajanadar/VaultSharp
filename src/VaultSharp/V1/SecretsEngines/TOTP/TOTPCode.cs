using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    /// <summary>
    /// Represents the TOTP Code.
    /// </summary>
    public class TOTPCode
    {
        /// <summary>
        /// Gets or sets the TOTP code.
        /// </summary>
        /// <value>
        /// The TOTP code.
        /// </value>
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}