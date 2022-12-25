using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The set of hash functions that are currently supported by Vault.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransitHashFunction
    {        
        SHA1,
        SHA224,
        SHA256,
        SHA384,
        SHA512
    }
}