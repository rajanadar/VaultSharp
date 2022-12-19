using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Role definition.
    /// </summary>
    public class CreateAWSRoleModel : AbstractAWSRoleModel
    {
        [JsonProperty("credential_type")]
        public AWSCredentialsType CredentialType { get; set; }
    }
}