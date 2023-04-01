using System.Collections.Generic;
using System.Text.Json.Serialization;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Token
{
    /// <summary>
    /// Token creation options.
    /// </summary>
    public class CreateTokenRequest
    {
        /// <summary>
        /// The ID of the client token. 
        /// Can only be specified by a root token. 
        /// Otherwise, the token ID is a randomly generated value.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the token role.
        /// </summary>
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }

        /// <summary>
        /// A list of policies for the token. 
        /// This must be a subset of the policies belonging to the token making the request, unless root. 
        /// If not specified, defaults to all the policies of the calling token.
        /// </summary>
        [JsonPropertyName("policies")]
        public IList<string> Policies { get; set; }

        /// <summary>
        /// A map of string to string valued metadata. 
        /// This is passed through to the audit devices.
        /// </summary>
        [JsonPropertyName("meta")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// When set to true, the token created will not have a parent.
        /// </summary>
        [JsonPropertyName("no_parent")]
        public bool NoParent { get; set; }
        
        /// <summary>
        /// When set to true, the token will be created via create-orphan endpoint.
        /// </summary>
        [JsonIgnore]
        public bool CreateOrphan { get; set; }

        /// <summary>
        /// If true the default policy will not be contained in this token's policy set.
        /// </summary>
        [JsonPropertyName("no_default_policy")]
        public bool NoDefaultPolicy { get; set; }

        /// <summary>
        /// Set to false to disable the ability of the token to be renewed past its initial TTL. 
        /// Setting the value to true will allow the token to be renewable up to the system/mount maximum TTL.
        /// </summary>
        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// The TTL period of the token, provided as "1h", where hour is the largest suffix. 
        /// If not provided, the token is valid for the default lease TTL, or indefinitely if the root policy is used.
        /// </summary>
        [JsonPropertyName("ttl")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public string TimeToLive { get; set; }

        /// <summary>
        /// The token type. Can be "batch" or "service". 
        /// Defaults to the type specified by the role configuration named by role_name.
        /// </summary>
        [JsonPropertyName("type")]
        public string TokenType { get; set; }

        /// <summary>
        ///  If set, the token will have an explicit max TTL set upon it. 
        ///  This maximum token TTL cannot be changed later, and unlike with normal tokens, 
        ///  updates to the system/mount max TTL value will have no effect at renewal time -- 
        ///  the token will never be able to be renewed or used past the value set at issue time.
        /// </summary>
        [JsonPropertyName("explicit_max_ttl")]
        public string ExplicitMaxTimeToLive { get; set; }

        /// <summary>
        /// The display name of the token.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The maximum uses for the given token. 
        /// This can be used to create a one-time-token or limited use token. 
        /// The value of 0 has no limit to the number of uses.
        /// </summary>
        [JsonPropertyName("num_uses")]
        public int NumberOfUses { get; set; }

        /// <summary>
        /// If specified, the token will be periodic; 
        /// it will have no maximum TTL (unless an "explicit-max-ttl" is also set) 
        /// but every renewal will use the given period. 
        /// Requires a root token or one with the sudo capability.
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; }

        /// <summary>
        /// Name of the entity alias to associate with during token creation. 
        /// Only works in combination with role_name argument and used entity alias 
        /// must be listed in allowed_entity_aliases. 
        /// If this has been specified, the entity will not be inherited from the parent.
        /// </summary>
        [JsonPropertyName("entity_alias")]
        public string EntityAlias { get; set; }

        public CreateTokenRequest()
        {
            Renewable = true;
        }
    }
}