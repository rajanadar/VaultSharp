using Newtonsoft.Json;

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
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}