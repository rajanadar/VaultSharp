using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Role definition.
    /// </summary>
    public class CreateAWSRoleModel : AbstractAWSRoleModel
    {
        [JsonPropertyName("credential_type")]
        public AWSCredentialsType CredentialType { get; set; }
    }
}