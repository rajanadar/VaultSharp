using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    /// <summary>
    /// Represents the TOTP Code validity.
    /// </summary>
    public class TOTPCodeValidity
    {
        /// <summary>
        /// Gets or sets the TOTO code validity.
        /// </summary>
        /// <value>
        /// The TOTP code validity.
        /// </value>
        [JsonProperty("valid")]
        public bool Valid { get; set; }
    }
}