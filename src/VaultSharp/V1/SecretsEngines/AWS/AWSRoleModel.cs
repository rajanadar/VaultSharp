using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    /// <summary>
    /// Role definition.
    /// </summary>
    public class AWSRoleModel : AbstractAWSRoleModel
    {
        [JsonProperty("credential_types")]
        public List<AWSCredentialsType> CredentialTypes { get; set; }
    }
}