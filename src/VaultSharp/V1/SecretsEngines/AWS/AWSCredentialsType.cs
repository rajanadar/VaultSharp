
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AWSCredentialsType
    {
        iam_user,
        assumed_role,
        federation_token
    }
}