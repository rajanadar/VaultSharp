
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<AWSCredentialsType>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum AWSCredentialsType
    {
        iam_user,
        assumed_role,
        federation_token
    }
}