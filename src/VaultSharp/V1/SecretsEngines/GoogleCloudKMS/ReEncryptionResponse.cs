using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// ReEncryption output.
    /// </summary>
    public class ReEncryptionResponse
    {
        /// <summary>
        ///  Integer version of the crypto key.
        /// </summary>
        /// <remarks>
        /// raja todo: why is this not int?
        /// </remarks>
        [JsonProperty("key_version")]
        public string KeyVersion { get; set; }

        /// <summary>
        /// ReEncrypted cipher text.
        /// </summary>
        [JsonProperty("ciphertext")]
        public string CipherText { get; set; }
    }
}