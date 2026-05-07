using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PrivateKeyType
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
    }
}