using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Cassandra
{
    /// <summary>
    /// Represents the Cassandra role definition with the creation, rollback query and lease information.
    /// </summary>
    public class CassandraRoleDefinition
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the CQL statements executed to create and configure the new user. 
        /// Must be semi-colon separated. The '{{username}}' and '{{password}}' values will be substituted; 
        /// it is required that these parameters are in single quotes. 
        /// The default creates a non-superuser user with no authorization grants.
        /// </summary>
        /// <value>
        /// The creation CQL.
        /// </value>
        [JsonProperty("creation_cql")]
        public string CreationCql { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the CQL statements executed to attempt a rollback if an error is encountered during user creation. 
        /// The default is to delete the user. Must be semi-colon separated. 
        /// The '{{username}}' and '{{password}}' values will be substituted; it is required that these parameters are in single quotes.
        /// </summary>
        /// <value>
        /// The rollback CQL.
        /// </value>
        [JsonProperty("rollback_cql")]
        public string RollbackCql { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the lease value provided as a string duration with time suffix. 
        /// Hour is the largest suffix.
        /// </summary>
        /// <value>
        /// The duration of the lease.
        /// </value>
        [JsonProperty("lease")]
        public string LeaseDuration { get; set; }
    }
}