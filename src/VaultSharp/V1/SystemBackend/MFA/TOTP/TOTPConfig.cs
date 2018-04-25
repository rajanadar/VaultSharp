using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend.MFA.TOTP
{
    /// <summary>
    /// TOTP Config.
    /// </summary>
    public class TOTPConfig : AbstractMFAConfig
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the key's issuing organization.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the length of time used to generate a counter for the TOTP token calculation.
        /// </summary>
        [JsonProperty("period")]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the size in bytes of the generated key.
        /// </summary>
        [JsonProperty("key_size")]
        public int KeySize  { get; set; }

        /// <summary>
        /// Gets or sets the pixel size of the generated square QR code.
        /// </summary>
        [JsonProperty("qr_size")]
        public int QRSize  { get; set; }

        /// <summary>
        /// Gets or sets the hashing algorithm used to generate the TOTP code. 
        /// Options include "SHA1", "SHA256" and "SHA512".
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the number of digits in the generated TOTP token. This value can either be 6 or 8.
        /// </summary>
        [JsonProperty("digits")]
        public int Digits { get; set; }

        /// <summary>
        /// Gets or sets the number of delay periods that are allowed when validating a TOTP token. 
        /// This value can either be 0 or 1.
        /// </summary>
        [JsonProperty("skew")]
        public int Skew { get; set; }

        /// <summary>
        /// Constructor with default values.
        /// </summary>
        public TOTPConfig()
        {
            Period = "30";
            KeySize = 20;
            QRSize = 200;
            Algorithm = "SHA1";
            Digits = 6;
            Skew = 1;
        }
    }
}