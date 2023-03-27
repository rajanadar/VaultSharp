using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Role definition.
    /// </summary>
    public class AWSRoleModel : AbstractAWSRoleModel
    {
        [JsonPropertyName("credential_types")]
        public List<AWSCredentialsType> CredentialTypes { get; set; }
    }
}