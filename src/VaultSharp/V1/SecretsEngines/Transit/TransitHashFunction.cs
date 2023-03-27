using System;

using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The set of hash functions that are currently supported by Vault.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransitHashFunction
    {        
        SHA1,
        SHA224,
        SHA256,
        SHA384,
        SHA512
    }
}