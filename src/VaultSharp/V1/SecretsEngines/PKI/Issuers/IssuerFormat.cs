using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<IssuerFormat>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum IssuerFormat
    {
        /// <summary>
        ///     Enum Pem for value: pem
        /// </summary>
        [EnumMember(Value = "pem")] pem = 1,

        /// <summary>
        ///     Enum Der for value: der
        /// </summary>
        [EnumMember(Value = "der")] der = 2,

        /// <summary>
        ///     Enum Pembundle for value: pem_bundle
        /// </summary>
        [EnumMember(Value = "pem_bundle")] pem_bundle = 3
    }

}