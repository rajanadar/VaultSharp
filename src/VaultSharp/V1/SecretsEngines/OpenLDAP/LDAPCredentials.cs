using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.OpenLDAP
{
    public class LDAPCredentials : UsernamePasswordCredentials
    {

        /// <summary>
        /// Specifies the Distinguished Names (DN) of the user
        /// </summary>
        [JsonPropertyName("distinguished_names")]
        public List<string> DistinguishedNames { get; set; }
        
    }
}