using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    /// <summary>
    /// Verification input.
    /// </summary>
    public class VerificationOptions
    {
        /// <summary>
        ///  Integer version of the crypto key version to use for verifying.
        /// </summary>
        [JsonPropertyName("key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        ///  Digest that was signed. 
        ///  This digest is the base64 encoded binary value, and must match the signing algorithm 
        ///  digest of the Cloud KMS key. 
        /// </summary>
        [JsonPropertyName("digest")]
        public string Digest { get; set; }

        /// <summary>
        /// Signature of the digest as returned from a signing operation.
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}