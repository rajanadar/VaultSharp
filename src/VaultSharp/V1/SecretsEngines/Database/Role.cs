using System.Collections.Generic;
using System.Text.Json.Serialization;
using VaultSharp.Core;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Role definition.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// The name of the database connection to use for this role.
        /// </summary>
        [JsonPropertyName("db_name")]
        public DatabaseProviderType DatabaseProviderType { get; set; }

        /// <summary>
        /// Specifies the TTL for the leases associated with this role. 
        /// Accepts time suffixed strings ("1h") or an integer number of seconds. 
        /// Defaults to system/engine default TTL time.
        /// </summary>
        [JsonPropertyName("default_ttl")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public string DefaultTimeToLive { get; set; }

        /// <summary>
        /// Specifies the maximum TTL for the leases associated with this role. 
        /// Accepts time suffixed strings ("1h") or an integer number of seconds. 
        /// Defaults to system/mount default TTL time; 
        /// this value is allowed to be less than the mount max TTL 
        /// (or, if not set, the system max TTL), but it is not allowed to be longer.
        /// </summary>
        [JsonPropertyName("max_ttl")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public string MaximumTimeToLive { get; set; }

        /// <summary>
        /// Specifies the database statements executed to create and configure a user.
        /// </summary>
        [JsonPropertyName("creation_statements")]
        public List<string> CreationStatements { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed to revoke a user.
        /// </summary>
        [JsonPropertyName("revocation_statements")]
        public List<string> RevocationStatements { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed rollback a create operation in the event of an error. 
        /// Not every plugin type will support this functionality. 
        /// </summary>
        [JsonPropertyName("rollback_statements")]
        public List<string> RollbackStatements { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed to renew a user. 
        /// Not every plugin type will support this functionality.
        /// </summary>
        [JsonPropertyName("renew_statements")]
        public List<string> RenewStatements { get; set; }
    }
}