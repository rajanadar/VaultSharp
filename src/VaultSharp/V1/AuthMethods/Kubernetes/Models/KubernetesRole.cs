using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.Kubernetes.Models;

public class KubernetesRole
{
    /// <summary>
    ///     required
    ///     Configures how identity aliases are generated. Valid choices are: serviceaccount_uid,   serviceaccount_name
    ///     When serviceaccount_uid is specified, the machine generated UID from the service account will be used as the
    ///     identity alias name.
    ///     When serviceaccount_name is specified, the service account's namespace and name will be used as the identity alias
    ///     name e.g vault/vault-auth.
    ///     While it is strongly advised that you use serviceaccount_uid, you may also use serviceaccount_name in cases where
    ///     you want to set the alias ahead of time,
    ///     and the risks are mitigated or otherwise acceptable given your use case. It is very important to limit who is able
    ///     to
    ///     delete/create service accounts within a given cluster. See the Create an Entity Alias document which further
    ///     expands on the potential
    ///     security implications mentioned above.
    /// </summary>
    [JsonProperty("alias_name_source")] public string? alias_name_source = "serviceaccount_uid";

    /// <summary>
    ///     Optional Audience claim to verify in the JWT.
    /// </summary>
    [JsonProperty("audience")] public string? audience;

    /// <summary>
    ///     required
    ///     List of service account names able to access this role. If set to "*" all names are allowed.
    /// </summary>
    [JsonProperty("bound_service_account_names")]
    public string[]? bound_service_account_names;

    /// <summary>
    ///     required
    ///     List of namespaces allowed to access this role. If set to "*" all namespaces are allowed.
    /// </summary>
    [JsonProperty("bound_service_account_namespaces")]
    public string[]? bound_service_account_namespaces;

    /// <summary>
    ///     required
    ///     Name of the role.
    /// </summary>
    [JsonProperty("name")] public string? name;

    /// <summary>
    ///     List of CIDR blocks; if set, specifies blocks of IP addresses which can authenticate successfully, and ties the
    ///     resulting token to these blocks as well.
    /// </summary>
    [JsonProperty("token_bound_cidrs")] public string[]? token_bound_cidrs;

    /// <summary>
    ///     If set, will encode an explicit max TTL onto the token. This is a hard cap even if token_ttl and token_max_ttl
    ///     would otherwise allow a renewal.
    /// </summary>
    [JsonProperty("token_explicit_max_ttl")]
    public string? token_explicit_max_ttl;

    /// <summary>
    ///     The maximum lifetime for generated tokens. This current value of this will be referenced at renewal time.
    /// </summary>
    [JsonProperty("token_max_ttl")] public string? token_max_ttl;

    /// <summary>
    ///     If set, the default policy will not be set on generated tokens; otherwise it will be added to the policies set in
    ///     token_policies.
    /// </summary>
    [JsonProperty("token_no_default_policy")]
    public bool token_no_default_policy;

    /// <summary>
    ///     The maximum number of times a generated token may be used (within its lifetime); 0 means unlimited.
    ///     If you require the token to have the ability to create child tokens, you will need to set this value to 0.
    /// </summary>
    [JsonProperty("token_num_uses")] public int token_num_uses;

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
    ///     (which unless changed will be service tokens).
    ///     For token store roles, there are two additional possibilities:
    ///     default-service and default-batch which specify the type to return unless the client requests a different type at
    ///     generation time.
    /// </summary>
    [JsonProperty("token_type")] public string? token_type;
}