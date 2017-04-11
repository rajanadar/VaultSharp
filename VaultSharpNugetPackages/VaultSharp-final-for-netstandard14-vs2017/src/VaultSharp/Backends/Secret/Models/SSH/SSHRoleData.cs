using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.SSH
{
    /// <summary>
    /// Represents the SSH Role data
    /// </summary>
    public class SSHRoleData
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSHRoleData"/> class.
        /// </summary>
        public SSHRoleData()
        {
            Roles = new List<string>();
        }
    }
}