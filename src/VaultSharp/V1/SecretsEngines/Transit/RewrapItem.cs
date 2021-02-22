using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents a single item that will be re-wrappred (re-encrypted).
    /// </summary>
    public class RewrapItem : DecryptionItem
    {
        /// <summary>
        /// Specifies the version of the key to use for the operation. 
        /// If not set, uses the latest version.
        /// </summary>
        [JsonProperty(PropertyName = "key_version")]
        public int KeyVersion { get; set; }
    }
}