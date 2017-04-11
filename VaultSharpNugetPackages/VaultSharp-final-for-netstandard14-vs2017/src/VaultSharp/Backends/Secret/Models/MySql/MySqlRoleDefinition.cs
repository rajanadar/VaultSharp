using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MySql
{
    /// <summary>
    /// Represents the MySql role definition
    /// </summary>
    public class MySqlRoleDefinition
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the SQL statements executed to create and configure the role. 
        /// Must be semi-colon separated string, a base64-encoded semicolon-separated string, 
        /// a serialized JSON string array, or a base64-encoded serialized JSON string array.
        /// The '{{name}}' and '{{password}}' values will be substituted.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        [JsonProperty("sql")]
        public string Sql { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the SQL statements executed to revoke a user. 
        /// Must be a semicolon-separated string, a base64-encoded semicolon-separated string, 
        /// a serialized JSON string array, or a base64-encoded serialized JSON string array. 
        /// The '{{name}}' value will be substituted.
        /// </summary>
        /// <value>
        /// The revocation SQL.
        /// </value>
        [JsonProperty("revocation_sql")]
        public string RevocationSql { get; set; }

        /// <summary>
        /// Gets or sets a value determining how many characters from the role name will be used to form the mysql 
        /// username interpolated into the '{{name}}' field of the sql parameter. 
        /// The default is 4.
        /// </summary>
        /// <value>
        /// The length of the role name.
        /// </value>
        [JsonProperty("rolename_length")]
        public int RoleNameLength { get; set; }

        /// <summary>
        /// Gets or sets a value determining how many characters from the token display name will be used to form the mysql 
        /// username interpolated into the '{{name}}' field of the sql parameter. 
        /// The default is 4.
        /// </summary>
        /// <value>
        /// The length of the role name.
        /// </value>
        [JsonProperty("displayname_length")]
        public int DisplayNameLength { get; set; }

        /// <summary>
        /// Gets or sets a value determining the maximum total length in characters of the mysql username 
        /// interpolated into the '{{name}}' field of the sql parameter. 
        /// The default is 16.
        /// </summary>
        /// <value>
        /// The length of the role name.
        /// </value>
        [JsonProperty("username_length")]
        public int UserNameLength { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlRoleDefinition"/> class.
        /// </summary>
        public MySqlRoleDefinition()
        {
            RoleNameLength = 4;
            DisplayNameLength = 4;
            UserNameLength = 16;
        }
    }
}