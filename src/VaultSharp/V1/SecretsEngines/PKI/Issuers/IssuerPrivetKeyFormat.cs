using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IssuerPrivateKeyFormat
    {
        /// <summary>
        ///     Enum Empty for value: null
        /// </summary>
        [EnumMember(Value = null)] Empty = 1,

        /// <summary>
        ///     Enum Der for value: der
        /// </summary>
        [EnumMember(Value = "der")] der = 2,

        /// <summary>
        ///     Enum Pem for value: pem
        /// </summary>
        [EnumMember(Value = "pem")] pem = 3,

        /// <summary>
        ///     Enum Pkcs8 for value: pkcs8
        /// </summary>
        [EnumMember(Value = "pkcs8")] pkcs8 = 4
    }
}
