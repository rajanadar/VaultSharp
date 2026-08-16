using System;

using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The set of hash functions that are currently supported by Vault.
    /// </summary>
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<TransitHashFunction>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum TransitHashFunction
    {        
        SHA1,
        SHA224,
        SHA256,
        SHA384,
        SHA512
    }
}