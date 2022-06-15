using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.GitHub.Models;

public class GitHubConfig
{
    /// <summary>
    ///     The API endpoint to use. Useful if you are running GitHub Enterprise or an API-compatible authentication server.
    /// </summary>
    [JsonProperty("base_url")] public string? base_url;

    /// <summary>
    ///     The organization users must be part of.
    /// </summary>
    [JsonProperty("organization")] public string organization = "";

    /// <summary>
    ///     The ID of the organization users must be part of.
    ///     Vault will attempt to fetch and set this value if it is not provided.
    /// </summary>
    [JsonProperty("organization_id")] public int? organization_id;

    /// <summary>
    ///     List of CIDR blocks; if set, specifies blocks of IP addresses which can authenticate successfully,
    ///     and ties the resulting token to these blocks as well.
    /// </summary>
    [JsonProperty("token_bound_cidrs")] public string[]? token_bound_cidrs;

    /// <summary>
    ///     If set, will encode an explicit max TTL onto the token.
    ///     This is a hard cap even if token_ttl and token_max_ttl would otherwise allow a renewal.
    /// </summary>
    [JsonProperty("token_explicit_max_ttl")]
    public string? token_explicit_max_ttl;

    /// <summary>
    ///     The maximum lifetime for generated tokens. This current value of this will be referenced at renewal time.
    /// </summary>
    [JsonProperty("token_max_ttl")] public string? token_max_ttl;

    /// <summary>
    ///     If set, the default policy will not be set on generated tokens;
    ///     otherwise it will be added to the policies set in token_policies.
    /// </summary>
    [JsonProperty("token_no_default_policy")]
    public bool? token_no_default_policy;

    /// <summary>
    ///     The maximum number of times a generated token may be used (within its lifetime); 0 means unlimited.
    ///     If you require the token to have the ability to create child tokens, you will need to set this value to 0.
    /// </summary>
    [JsonProperty("token_num_uses")] public int? token_num_uses;

    /// <summary>
    ///     The period, if any, to set on the token.
    /// </summary>
    [JsonProperty("token_period")] public string? token_period;

    /// <summary>
    ///     List of policies to encode onto generated tokens. Depending on the auth method, this list may be supplemented by
    ///     user/group/other values.
    /// </summary>
    [JsonProperty("token_policies")] public string[]? token_policies;

    /// <summary>
    ///     The incremental lifetime for generated tokens. This current value of this will be referenced at renewal time.
    /// </summary>
    [JsonProperty("token_ttl")] public string? token_ttl;


    /// <summary>
    ///     The type of token that should be generated. Can be service, batch, or default to use the mount's tuned default
    ///     (which unless changed will be service tokens). For token store roles, there are two additional possibilities:
    ///     default-service and default-batch which specify the type to return unless the client requests a different type at
    ///     generation time.
    /// </summary>
    [JsonProperty("token_type")] public string? token_type;
}