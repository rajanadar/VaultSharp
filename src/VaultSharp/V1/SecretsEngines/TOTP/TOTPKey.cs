using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    /// <summary>
    /// Represents a queried key
    /// </summary>
    public class TOTPKey
    {
        /// <summary>
        /// Gets or sets the name of the account associated with the key.
        /// </summary>
        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the used hashing algorithm.
        /// </summary>
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the number of digits in the generated TOTP code.
        /// This value can be set to 6 or 8.
        /// </summary>
        [JsonPropertyName("digits")]
        public int Digits { get; set; }

        /// <summary>
        /// Gets or sets the name of the issuing organization.
        /// </summary>
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the length of time in seconds used to
        /// create a counter for the TOTP code calculation.
        /// </summary>
        [JsonPropertyName("period")]
        public long Period { get; set; }
    }
}
