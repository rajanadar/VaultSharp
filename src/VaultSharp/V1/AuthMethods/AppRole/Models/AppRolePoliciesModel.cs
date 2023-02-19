using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace VaultSharp.V1.AuthMethods.AppRole.Models
{
    public class AppRolePoliciesModel
    {
        [JsonPropertyName("policies")]
        public List<string> Policies { get; set; }

        [JsonPropertyName("token_policies")]
        public List<string> TokenPolicies { get; set; }
    }
}