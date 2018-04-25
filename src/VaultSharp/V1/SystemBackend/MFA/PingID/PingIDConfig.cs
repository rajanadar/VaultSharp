using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend.MFA.PingID
{
    /// <summary>
    /// PingID Config.
    /// </summary>
    public class PingIDConfig : AbstractMFAConfig
    {
        /// <summary>
        /// Gets or sets the mount to tie this method to for use in automatic mappings. 
        /// The mapping will use the Name field of Aliases associated with this mount as the username in the mapping.
        /// </summary>
        [JsonProperty("mount_accessor")]
        public string MountAccessor { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the format string for mapping Identity names to MFA method names. 
        /// Values to substitute should be placed in {{}}. 
        /// For example, "{{alias.name}}@example.com". 
        /// If blank, the Alias's Name field will be used as-is. 
        /// Currently-supported mappings:
        ///     alias.name: The name returned by the mount configured via the mount_accessor parameter
        ///     entity.name: The name configured for the Entity
        ///     alias.metadata.[key]: The value of the Alias's metadata parameter
        ///     entity.metadata.[key]: The value of the Entity's metadata paramater
        /// </summary>
        [JsonProperty("username_format")]
        public string UsernameFormat { get; set; }

        /// <summary>
        /// Gets or sets a base64-encoded third-party settings file retrieved from PingID's configuration page.
        /// </summary>
        [JsonProperty("settings_file_base64")]
        public string Base64SettingsFile { get; set; }

        /// <summary>
        /// Gets or sets a flag to use signature.
        /// </summary>
        [JsonProperty("use_signature")]
        public bool UseSignature { get; set; }

        /// <summary>
        /// Gets or sets IDP url.
        /// </summary>
        [JsonProperty("idp_url")]
        public string IdpUrl { get; set; }

        /// <summary>
        /// Gets or sets the admin url.
        /// </summary>
        [JsonProperty("admin_url")]
        public string AdminUrl { get; set; }

        /// <summary>
        /// Gets or sets the authenticator url.
        /// </summary>
        [JsonProperty("authenticator_url")]
        public string AuthenticatorUrl { get; set; }

        /// <summary>
        /// Gets or sets the Org Alias.
        /// </summary>
        [JsonProperty("org_alias")]
        public string OrgAlias { get; set; }
    }
}