

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The set of hash algorithms that are currently supported by Vault.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransitHashAlgorithm
    {
        [Obsolete]
        [EnumMember(Value = "sha1")]
        SHA1,

        [EnumMember(Value = "sha2-224")]
        SHA2_224,

        [EnumMember(Value = "sha2-256")]
        SHA2_256,

        [EnumMember(Value = "sha2-384")]
        SHA2_384,

        [EnumMember(Value = "sha2-512")]
        SHA2_512,

        [EnumMember(Value = "sha3-224")]
        SHA3_224,

        [EnumMember(Value = "sha3-256")]
        SHA3_256,

        [EnumMember(Value = "sha3-384")]
        SHA3_384,

        [EnumMember(Value = "sha3-512")]
        SHA3_512
    }
}