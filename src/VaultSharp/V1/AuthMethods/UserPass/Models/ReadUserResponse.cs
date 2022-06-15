using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.UserPass.Models;

public class ReadUserResponse
{
    /// <summary>
    ///     The incremental lifetime for generated tokens.This current value of this will be referenced at renewal time.
    /// </summary>
    [JsonProperty("token_ttl")]
    public string? TokenTtl { get; set; }

    /// <summary>
    ///     The maximum lifetime for generated tokens.This current value of this will be referenced at renewal time.
    /// </summary>
    [JsonProperty("token_max_ttl")]
    public string? TokenMaxTtl { get; set; }

    /// <summary>
    ///     List of policies to encode onto generated tokens.Depending on the auth method, this list may be supplemented by
    ///     user/group/other values.
    /// </summary>
    [JsonProperty("token_policies")]
    public List<string>? TokenPolicies { get; set; }

    /// <summary>
    ///     List of CIDR blocks; if set, specifies blocks of IP addresses which can authenticate successfully, and ties the
    ///     resulting token to these blocks as well.
    /// </summary>
    [JsonProperty("token_bound_cidrs")]
    public List<string>? TokenBoundCidrs { get; set; }

    /// <summary>
    ///     If set, will encode an explicit max TTL onto the token.This is a hard cap even if token_ttl and token_max_ttl would
    ///     otherwise allow a renewal.
    /// </summary>
    [JsonProperty("token_explicit_max_ttl")]
    public string? TokenExplicitMaxTtl { get; set; }

    /// <summary>
    ///     If set, the default policy will not be set on generated tokens; otherwise it will be added to the policies set in
    ///     token_policies.
    /// </summary>
    [JsonProperty("token_no_default_policy")]
    public bool TokenNoDefaultPolicy { get; set; }

    /// <summary>
    ///     The maximum number of times a generated token may be used(within its lifetime); 0 means unlimited.If you require
    ///     the token to have the ability to create child tokens, you will need to set this value to 0.
    /// </summary>
    [JsonProperty("token_num_uses")]
    public int TokenNumUses { get; set; }

    /// <summary>
    ///     The period, if any, to set on the token.
    /// </summary>
    [JsonProperty("token_period")]
    public string? TokenPeriod { get; set; }

    /// <summary>
    ///     The type of token that should be generated.Can be service, batch, or default to use the mount's tuned default
    ///     (which unless changed will be service tokens). For token store roles, there are two additional possibilities:
    ///     default-service and default-batch which specify the type to return unless the client requests a different type at
    ///     generation time.
    /// </summary>
    [JsonProperty("token_type")]
    public string? TokenType { get; set; }
}