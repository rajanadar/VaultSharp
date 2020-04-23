using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    /// <summary>
    /// Represents the result when creating a new key
    /// </summary>
    public class TOTPProvider
    {
        /// <summary>
        /// Gets or sets the Barcode
        /// </summary>
        /// <remarks>
        /// If a QR code is returned, it consists of base64-formatted PNG bytes.
        /// You can embed it in a web page by including the base64 string
        /// in an 'img'-tag with the prefix data:image/png;base64
        /// </remarks>
        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Gets or sets the Url
        /// </summary>
        /// <remarks>
        /// The Url can be used by the client application in order to create
        /// TOTP codes.
        /// </remarks>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
