using Newtonsoft.Json;

namespace VaultSharp.Backends.Authentication.Models.Token
{
    /// <summary>
    /// Represents the information associated with a token role.
    /// </summary>
    public abstract class TokenRoleBase
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [JsonProperty("name")]
        public string RoleName { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether this <see cref="TokenRoleInfo"/> is orphan.
        /// If true, tokens created against this policy will be orphan tokens 
        /// (they will have no parent). As such, they will not be automatically revoked 
        /// by the revocation of any other token.
        /// </summary>
        /// <value>
        ///   <c>true</c> if orphan; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("orphan")]
        public bool Orphan { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the period seconds.
        /// If set, tokens created against this role will not have a maximum lifetime. 
        /// Instead, they will have a fixed TTL that is refreshed with each renewal. 
        /// So long as they continue to be renewed, they will never expire. 
        /// The parameter is an integer duration of seconds. 
        /// Tokens issued track updates to the role value; the new period takes effect upon next renew. 
        /// This cannot be used in conjunction with <see cref="ExplicitMaximumTimeToLive" />        
        /// </summary>
        /// <value>
        /// The period seconds.
        /// </value>
        [JsonProperty("period")]
        public int PeriodSeconds { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether this <see cref="TokenRoleInfo"/> is renewable.
        /// Set to false to disable the ability of token created against this role to be renewed 
        /// past their initial TTL. Defaults to true, which allows tokens to be renewed up 
        /// to the <see cref="IVaultClient.TuneAuthenticationBackendConfigurationAsync"/> maximum TimeToLive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renewable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the path suffix.
        /// If set, tokens created against this role will have the given suffix as part of 
        /// their path in addition to the role name. 
        /// This can be useful in certain scenarios, such as keeping the same role name 
        /// in the future but revoking all tokens created against it before some 
        /// point in time. The suffix can be changed, allowing new callers to have the 
        /// new suffix as part of their path, and then tokens with the old suffix 
        /// can be revoked via <see cref="IVaultClient.RevokeAllSecretsOrTokensUnderPrefixAsync"/>.
        /// </summary>
        /// <value>
        /// The path suffix.
        /// </value>
        [JsonProperty("path_suffix")]
        public string PathSuffix { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the explicit maximum time to live.
        /// If set, tokens created with this role have an explicit max TTL set upon them. 
        /// This maximum token TTL cannot be changed later, and unlike with normal tokens, 
        /// updates to the role or the <see cref="IVaultClient.TuneAuthenticationBackendConfigurationAsync"/> maximum TimeToLive value will have no effect at 
        /// renewal time -- the token will never be able to be renewed or used past 
        /// the value set at issue time. This cannot be used in conjunction with <see cref="PeriodSeconds"/>.
        /// </summary>
        /// <value>
        /// The explicit maximum time to live.
        /// </value>
        [JsonProperty("explicit_max_ttl")]
        public int ExplicitMaximumTimeToLive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRoleBase"/> class.
        /// </summary>
        public TokenRoleBase()
        {
            Renewable = true;
        }
    }
}