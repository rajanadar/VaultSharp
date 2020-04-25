using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Signature input.
    /// </summary>
    public class SignatureOptions
    {
        /// <summary>
        ///  Integer version of the crypto key version to use for signing. 
        /// </summary>
        [JsonProperty("key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        ///  Digest to sign. 
        ///  This digest is the base64 encoded binary value, 
        ///  and must match the signing algorithm digest of the Cloud KMS key,
        /// </summary>
        [JsonProperty("digest")]
        public string Digest { get; set; }
    }
}