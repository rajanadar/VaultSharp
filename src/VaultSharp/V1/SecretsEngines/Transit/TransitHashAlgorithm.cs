
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System;
using System.Text.Json;
using VaultSharp.Core;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The set of hash algorithms that are currently supported by Vault.
    /// </summary>
    [JsonConverter(typeof(TransitHashAlgorithmEnumConverter))]
    public enum TransitHashAlgorithm
    {
        [Obsolete]
        [EnumMember(Value = "sha1")]    // CRITICAL: doesn't work for System.Text.Json
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

    internal sealed class TransitHashAlgorithmEnumConverter : JsonConverter<TransitHashAlgorithm>
    {
        public override TransitHashAlgorithm Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();

            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                return (TransitHashAlgorithm)Enum.Parse(typeof(TransitHashAlgorithm), stringValue.ToUpperInvariant().Replace("-", "_"));
            }
            

            throw new VaultApiException("TransitHashAlgorithm was null");
        }

        public override void Write(Utf8JsonWriter writer, TransitHashAlgorithm value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLowerInvariant().Replace("_", "-"));
        }
    }
}