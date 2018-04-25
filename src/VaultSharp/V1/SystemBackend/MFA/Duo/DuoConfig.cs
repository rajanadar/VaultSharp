using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend.MFA.Duo
{
    /// <summary>
    /// Duo Config.
    /// </summary>
    public class DuoConfig : AbstractMFAConfig
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
        /// Gets or sets the SecretEngine key for Duo.
        /// </summary>
        [JsonProperty("secret_key")]
        public string SecretKey  { get; set; }

        /// <summary>
        /// Gets or sets the Integration key for Duo.
        /// </summary>
        [JsonProperty("integration_key")]
        public string IntegrationKey { get; set; }

        /// <summary>
        /// Gets or sets the API hostname for Duo.
        /// </summary>
        [JsonProperty("api_hostname")]
        public string ApiHostname { get; set; }

        /// <summary>
        /// Gets or sets the Push information for Duo.
        /// </summary>
        [JsonProperty("push_info")]
        public string PushInfo { get; set; }
    }
}