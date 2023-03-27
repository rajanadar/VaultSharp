using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.Token.Models
{
    /// <summary>
    /// Configuration of named role.
    /// </summary>
    public class TokenRoleInfo
    {
        /// <summary>
        /// Name of the token role.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// If true, tokens created against this policy will be orphan tokens 
        /// (they will have no parent). As such, they will not be automatically
        /// revoked by the revocation of any other token.
        /// </summary>
        [JsonPropertyName("orphan")]
        public bool Orphan { get; set; }

        /// <summary>
        /// If set, tokens created against this role will have the given suffix 
        /// as part of their path in addition to the role name. This can be 
        /// useful in certain scenarios, such as keeping the same role name in 
        /// the future but revoking all tokens created against it before some 
        /// point in time. The suffix can be changed, allowing new callers to have 
        /// the new suffix as part of their path, and then tokens with the old 
        /// suffix can be revoked via /sys/leases/revoke-prefix
        /// </summary>
        [JsonPropertyName("path_suffix")]
        public string PathSuffix { get; set; }

        [JsonPropertyName("period")]
        public string Period { get; set; }

        /// <summary>
        /// Set to false to disable the ability of the token to be renewed past
        /// its initial TTL. Setting the value to true will allow the token to 
        /// be renewable up to the system/mount maximum TTL.
        /// </summary>
        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// If set, will encode an explicit max TTL onto the token. This is a 
        /// hard cap even if token_ttl and token_max_ttl would otherwise allow 
        /// a renewal.
        /// </summary>
        [JsonPropertyName("token_explicit_max_ttl")]
        public string TokenExplicitMaxTimeToLive { get; set; }

        /// <summary>
        /// If set, the default policy will not be set on generated tokens; 
        /// otherwise it will be added to the policies set in token_policies.
        /// </summary>
        [JsonPropertyName("token_no_default_policy")]
        public bool TokenNoDefaultPolicy { get; set; }

        /// <summary>
        /// Period if anything is set on the token.
        /// </summary>
        [JsonPropertyName("token_period")]
        public string TokenPeriod { get; set; }

        /// <summary>
        /// The type of token to be generated. Can be one of service, batch or
        /// default. For token store roles, there are two additional 
        /// possibilities: default-service and default-batch which specify the 
        /// type to return unless the client requests a different type at 
        /// generation time.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Explicit Max TTL of the role.
        /// </summary>
        [JsonPropertyName("explicit_max_ttl")]
        public string ExplicitMaxTimeToLive { get; set; }

        /// <summary>
        /// String or JSON list of allowed entity aliases. If set, specifies 
        /// the entity aliases which are allowed to be used during token 
        /// generation. This field supports globbing. 
        /// Note that allowed_entity_aliases is not case sensitive.
        /// </summary>
        [JsonPropertyName("allowed_entity_aliases")]
        public IList<string> AllowedEntityAliases { get; set; }

        /// <summary>
        ///  If set, tokens can be created with any subset of the policies in 
        ///  this list, rather than the normal semantics of tokens being a 
        ///  subset of the calling token's policies. The parameter is a 
        ///  comma-delimited string of policy names. If at creation time 
        ///  no_default_policy is not set and "default" is not contained in 
        ///  disallowed_policies or glob matched in disallowed_policies_glob, 
        ///  the "default" policy will be added to the created token 
        ///  automatically.
        /// </summary>
        [JsonPropertyName("allowed_policies")]
        public IList<string> AllowedPolcies { get; set; }

        /// <summary>
        /// If set, successful token creation via this role will require that 
        /// no policies in the given list are requested. The parameter is a 
        /// comma-delimited string of policy names. Adding "default" to this 
        /// list will prevent "default" from being added automatically to 
        /// created tokens.
        /// </summary>
        [JsonPropertyName("disallowed_policies")]
        public IList<string> DisallowedPolcies { get; set; }

        /// <summary>
        /// If set, tokens can be created with any subset of glob matched 
        /// policies in this list, rather than the normal semantics of tokens 
        /// being a subset of the calling token's policies. The parameter is a 
        /// comma-delimited string of policy name globs. If at creation time 
        /// no_default_policy is not set and "default" is not contained in 
        /// disallowed_policies or glob matched in disallowed_policies_glob, 
        /// the "default" policy will be added to the created token 
        /// automatically. 
        /// If combined with allowed_policies policies need to only match one 
        /// of the two lists to be permitted. Note that unlike allowed_policies 
        /// the policies listed in allowed_policies_glob will not be added to 
        /// the token when no policies are specified in the call to 
        /// /auth/token/create/:role_name.
        /// </summary>
        [JsonPropertyName("allowed_policies_glob")]
        public IList<string> AllowedPolciesGlob { get; set; }

        /// <summary>
        ///  If set, successful token creation via this role will require that no requested policies glob match any of policies in this list. The parameter is a comma-delimited string of policy name globs. Adding any glob that matches "default" to this list will prevent "default" from being added automatically to created tokens. If combined with disallowed_policies policies need to only match one of the two lists to be blocked.
        /// </summary>
        [JsonPropertyName("disallowed_policies_glob")]
        public IList<string> DisallowedPolciesGlob { get; set; }
    }
}
