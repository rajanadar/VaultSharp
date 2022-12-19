using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AWSCredentialsType
    {
        iam_user,
        assumed_role,
        federation_token
    }
}