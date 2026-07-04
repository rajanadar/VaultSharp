using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Roles
{
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<PrivateKeyRoleType>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum PrivateKeyRoleType
    {
        /// <summary>
        ///     Enum Rsa for value: rsa
        /// </summary>
        [EnumMember(Value = "rsa")] rsa = 1,

        /// <summary>
        ///     Enum Ec for value: ec
        /// </summary>
        [EnumMember(Value = "ec")] ec = 2,

        /// <summary>
        ///     Enum Ed25519 for value: ed25519
        /// </summary>
        [EnumMember(Value = "ed25519")] ed25519 = 3,

        /// <summary>
        /// Enum Any for value: any
        /// </summary>
        [EnumMember(Value = "any")]
        any = 4
    }
}