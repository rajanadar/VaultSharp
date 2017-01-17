using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.Backends.Secret.Models.Transit
{
    /// <summary>
    /// Represents the type of Transit key to be generated.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransitKeyType
    {
        /// <summary>
        /// AES-256 wrapped with GCM using a 12-byte nonce size (symmetric)
        /// </summary>
        [EnumMember(Value = "aes256-gcm96")]
        aes256_gcm96 = 1,

        /// <summary>
        /// ECDSA using the P-256 elliptic curve (asymmetric)
        /// </summary>
        [EnumMember(Value = "ecdsa-p256")]
        ecdsa_p256 = 2
    }
}