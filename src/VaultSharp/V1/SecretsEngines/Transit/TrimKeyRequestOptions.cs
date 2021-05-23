using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the options that will be used to trim the key.
    /// </summary>
    public class TrimKeyRequestOptions
    {
        /// <summary>
        /// Specifies the minimum_available_version.The minimum available 
        /// version for the key ring. All versions before this version will be
        /// permanently deleted. This value can at most be equal to the lesser 
        /// of min_decryption_version and min_encryption_version.
        /// </summary>
        [JsonProperty(PropertyName = "min_available_version")]
        public int MinimumAvailableVersion { get; set; }
    }
}
