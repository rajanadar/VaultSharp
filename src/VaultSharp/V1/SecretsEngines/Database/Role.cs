using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("db_name")]
        public DatabaseProviderType DatabaseProviderType { get; set; }

        /// <summary>
        /// Specifies the TTL for the leases associated with this role. 
        /// Accepts time suffixed strings ("1h") or an integer number of seconds. 
        /// Defaults to system/engine default TTL time.
        /// </summary>
        [JsonProperty("default_ttl")]
        public string DefaultTimeToLive { get; set; }

        /// <summary>
        /// Specifies the maximum TTL for the leases associated with this role. 
        /// Accepts time suffixed strings ("1h") or an integer number of seconds. 
        /// Defaults to system/mount default TTL time; 
        /// this value is allowed to be less than the mount max TTL 
        /// (or, if not set, the system max TTL), but it is not allowed to be longer.
        /// </summary>
        [JsonProperty("max_ttl")]
        public string MaximumTimeToLive { get; set; }

        /// <summary>
        /// Specifies the database statements executed to create and configure a user.
        /// </summary>
        [JsonProperty("creation_statements")]
        public List<string> CreationStatements { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed to revoke a user.
        /// </summary>
        [JsonProperty("revocation_statements")]
        public List<string> RevocationStatements { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed rollback a create operation in the event of an error. 
        /// Not every plugin type will support this functionality. 
        /// </summary>
        [JsonProperty("rollback_statements")]
        public List<string> RollbackStatements { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed to renew a user. 
        /// Not every plugin type will support this functionality.
        /// </summary>
        [JsonProperty("renew_statements")]
        public List<string> RenewStatements { get; set; }
    }
}