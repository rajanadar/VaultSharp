using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.OpenLDAP
{
    public class Role
    {
        /// <summary>
        /// A templatized LDIF string used to create a user account.
        /// </summary>
        /// <remarks>
        /// This may contain multiple LDIF entries. The creation_ldif can also be used to add the
        /// user account to an existing group. All LDIF entries are performed in order. If Vault
        /// encounters an error while executing the creation_ldif it will stop at the first error
        /// and not execute any remaining LDIF entries. If an error occurs and rollback_ldif is
        /// specified, the LDIF entries in rollback_ldif will be executed. See rollback_ldif for
        /// more details. This field may optionally be provided as a base64 encoded string.
        /// </remarks>
        [JsonPropertyName("creation_ldif")]
        public string CreationLdif { get; set; }

        /// <summary>
        /// A templatized LDIF string used to delete the user account once its TTL has expired.
        /// </summary>
        /// <remarks>
        ///  This may contain multiple LDIF entries. All LDIF entries are performed in order.
        /// If Vault encounters an error while executing an entry in the deletion_ldif it will
        /// attempt to continue executing any remaining entries. This field may optionally be
        /// provided as a base64 encoded string.
        /// </remarks>
        [JsonPropertyName("deletion_ldif")]
        public string DeletionLdif { get; set; }

        /// <summary>
        /// A templatized LDIF string used to attempt to rollback any changes in the event that
        /// execution of the creation_ldif results in an error
        /// </summary>
        /// <remarks>
        /// This may contain multiple LDIF entries. All LDIF entries are performed in order.
        /// If Vault encounters an error while executing an entry in the rollback_ldif it will
        /// attempt to continue executing any remaining entries. This field may optionally be
        /// provided as a base64 encoded string.
        /// </remarks>
        [JsonPropertyName("rollback_ldif")]
        public string RollbackLdif { get; set; }

        /// <summary>
        /// A template used to generate a dynamic username. This will be used to fill in
        /// the .Username field within the creation_ldif string.
        /// </summary>
        [JsonPropertyName("username_template")]
        public string UsernameTemplate { get; set; }
    }
}