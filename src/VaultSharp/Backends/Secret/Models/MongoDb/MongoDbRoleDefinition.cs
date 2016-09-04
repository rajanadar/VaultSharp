using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MongoDb
{
    /// <summary>
    /// Represents the MongoDb role definition
    /// </summary>
    public class MongoDbRoleDefinition
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets name of the database users should be created in for this role.
        /// </summary>
        /// <value>
        /// The database name.
        /// </value>
        [JsonProperty("db")]
        public string Database { get; set; }

        // raja todo: make this more friendly, and string should be the output type.

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the MongoDb roles. You specify both built-in roles and user-defined roles 
        /// for both the database the user is created in and for other databases.
        /// e.g. Roles = JsonConvert.SerializeObject(new object[] { "readWrite", new { role = "read", db = "bar" } }) 
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [JsonProperty("roles")]
        public string Roles { get; set; }
    }
}