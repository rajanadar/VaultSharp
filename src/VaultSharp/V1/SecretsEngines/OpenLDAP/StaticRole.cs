using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.SecretsEngines.Database;

namespace VaultSharp.V1.SecretsEngines.OpenLDAP
{
    /// <summary>
    /// Static role definition.
    /// </summary>
    public class StaticRole
    {

        /// <summary>
        /// The username of the existing LDAP entry to manage password rotation for.
        /// </summary>
        /// <remarks>
        /// LDAP search for the username will be rooted at the `userdn` configuration value.
        /// The attribute to use when searching for the user can be configured with the
        /// `userattr` configuration value. This is useful when `dn` isn't used for login
        /// purposes (such as SSH). Cannot be modified after creation.
        /// </remarks>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Specifies the database username that this Vault role corresponds to.
        /// </summary>
        [JsonPropertyName("dn")]
        public string DistinguishedName { get; set; }

        /// <summary>
        /// Specifies the amount of time Vault should wait before rotating the password. 
        /// The minimum is 5 seconds.
        /// </summary>
        [JsonPropertyName("rotation_period")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public string RotationPeriod { get; set; }
    }
}
