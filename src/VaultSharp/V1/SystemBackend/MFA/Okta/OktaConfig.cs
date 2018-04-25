using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend.MFA.Okta
{
    /// <summary>
    /// Okta Config.
    /// </summary>
    public class OktaConfig : AbstractMFAConfig
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
        /// Gets or sets the name of the organization to be used in the Okta API.
        /// </summary>
        [JsonProperty("org_name")]
        public string OrgName { get; set; }

        /// <summary>
        /// Gets or sets the Okta Api Key.
        /// </summary>
        [JsonProperty("api_token")]
        public string ApiToken { get; set; }

        /// <summary>
        /// Gets or sets a flag when set, will be used as the base domain for API requests. 
        /// Examples are okta.com, oktapreview.com, and okta-emea.com.
        /// </summary>
        [JsonProperty("base_url")]
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this is production.
        /// </summary>
        [JsonProperty("production")]
        public bool Production { get; set; }
    }
}