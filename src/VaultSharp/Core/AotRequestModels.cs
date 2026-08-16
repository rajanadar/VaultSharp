using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.Core
{
    internal sealed class TokenRequest
    {
        [JsonPropertyName("token")] public string Token { get; set; }
    }

    internal sealed class TokenClientIdRequest
    {
        [JsonPropertyName("token")] public string Token { get; set; }
        [JsonPropertyName("client_id")] public string ClientId { get; set; }
    }

    internal sealed class InputRequest
    {
        [JsonPropertyName("input")] public string Input { get; set; }
    }

    internal sealed class PathTokenRequest
    {
        [JsonPropertyName("path")] public string Path { get; set; }
        [JsonPropertyName("token")] public string Token { get; set; }
    }

    internal sealed class PathAccessorRequest
    {
        [JsonPropertyName("path")] public string Path { get; set; }
        [JsonPropertyName("accessor")] public string Accessor { get; set; }
    }

    internal sealed class PathRequest
    {
        [JsonPropertyName("path")] public string Path { get; set; }
    }

    internal sealed class OtpPgpKeyRequest
    {
        [JsonPropertyName("otp")] public string Otp { get; set; }
        [JsonPropertyName("pgpKey")] public string PgpKey { get; set; }
    }

    internal sealed class KeyNonceRequest
    {
        [JsonPropertyName("key")] public string Key { get; set; }
        [JsonPropertyName("nonce")] public string Nonce { get; set; }
    }

    internal sealed class ValueRequest
    {
        [JsonPropertyName("value")] public string Value { get; set; }
    }

    internal sealed class RekeyInitRequest
    {
        [JsonPropertyName("secret_shares")] public int SecretShares { get; set; }
        [JsonPropertyName("secret_threshold")] public int SecretThreshold { get; set; }
        [JsonPropertyName("pgp_keys")] public string[] PgpKeys { get; set; }
        [JsonPropertyName("backup")] public bool Backup { get; set; }
    }

    internal sealed class KeyResetRequest
    {
        [JsonPropertyName("key")] public string Key { get; set; }
        [JsonPropertyName("reset")] public bool Reset { get; set; }
    }

    internal sealed class MaxTtlRequest
    {
        [JsonPropertyName("max_ttl")] public string MaxTtl { get; set; }
    }

    internal sealed class AccessorRequest
    {
        [JsonPropertyName("accessor")] public string Accessor { get; set; }
    }

    internal sealed class CodeRequest
    {
        [JsonPropertyName("code")] public string Code { get; set; }
    }

    internal sealed class IpUsernameRequest
    {
        [JsonPropertyName("ip")] public string Ip { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; }
    }

    internal sealed class SerialNumberRequest
    {
        [JsonPropertyName("serial_number")] public string SerialNumber { get; set; }
    }

    internal sealed class CasRequest
    {
        [JsonPropertyName("cas")] public int Cas { get; set; }
    }

    internal sealed class VersionsRequest
    {
        [JsonPropertyName("versions")] public System.Collections.Generic.IList<int> Versions { get; set; }
    }

    internal sealed class FormatRequest
    {
        [JsonPropertyName("format")] public string Format { get; set; }
    }

    internal sealed class TtlRequest
    {
        [JsonPropertyName("ttl")] public long? Ttl { get; set; }
    }

    internal sealed class ServiceAccountNamesRequest
    {
        [JsonPropertyName("service_account_names")] public List<string> ServiceAccountNames { get; set; }
    }

    internal sealed class IncrementRequest
    {
        [JsonPropertyName("increment")] public string Increment { get; set; }
    }

    internal sealed class PoliciesRequest
    {
        [JsonPropertyName("policies")] public string Policies { get; set; }
    }

    internal sealed class PoliciesGroupsRequest
    {
        [JsonPropertyName("policies")] public string Policies { get; set; }
        [JsonPropertyName("groups")] public string Groups { get; set; }
    }

    internal sealed class NameRequest
    {
        [JsonPropertyName("name")] public string Name { get; set; }
    }

    internal sealed class SecretIdRequest
    {
        [JsonPropertyName("secret_id")] public string SecretId { get; set; }
    }

    internal sealed class SecretIdAccessorRequest
    {
        [JsonPropertyName("secret_id_accessor")] public string SecretIdAccessor { get; set; }
    }

    internal sealed class SecretIdNumUsesRequest
    {
        [JsonPropertyName("secret_id_num_uses")] public long SecretIdNumUses { get; set; }
    }

    internal sealed class SecretIdTtlRequest
    {
        [JsonPropertyName("secret_id_ttl")] public long SecretIdTtl { get; set; }
    }

    internal sealed class TokenTtlRequest
    {
        [JsonPropertyName("token_ttl")] public long TokenTtl { get; set; }
    }

    internal sealed class TokenMaxTtlRequest
    {
        [JsonPropertyName("token_max_ttl")] public long TokenMaxTtl { get; set; }
    }

    internal sealed class BindSecretIdRequest
    {
        [JsonPropertyName("bind_secret_id")] public bool BindSecretId { get; set; }
    }

    internal sealed class SecretIdBoundCidrsRequest
    {
        [JsonPropertyName("secret_id_bound_cidrs")] public List<string> SecretIdBoundCidrs { get; set; }
    }

    internal sealed class TokenBoundCidrsRequest
    {
        [JsonPropertyName("token_bound_cidrs")] public List<string> TokenBoundCidrs { get; set; }
    }

    internal sealed class TokenPeriodRequest
    {
        [JsonPropertyName("token_period")] public long TokenPeriod { get; set; }
    }

    internal sealed class HmacRequest
    {
        [JsonPropertyName("hmac")] public bool Hmac { get; set; }
    }

    internal sealed class LeaseIdRequest
    {
        [JsonPropertyName("lease_id")] public string LeaseId { get; set; }
    }

    internal sealed class LeaseRenewRequest
    {
        [JsonPropertyName("lease_id")] public string LeaseId { get; set; }
        [JsonPropertyName("increment")] public int Increment { get; set; }
    }

    internal sealed class LevelRequest
    {
        [JsonPropertyName("level")] public string Level { get; set; }
    }

    internal sealed class RulesRequest
    {
        [JsonPropertyName("rules")] public string Rules { get; set; }
    }

    internal sealed class PolicyTextRequest
    {
        [JsonPropertyName("policy")] public string Policy { get; set; }
    }

    internal sealed class CloudFoundryLoginRequest
    {
        [JsonPropertyName("role")] public string Role { get; set; }
        [JsonPropertyName("cf_instance_cert")] public string CfInstanceCert { get; set; }
        [JsonPropertyName("signing_time")] public string SigningTime { get; set; }
        [JsonPropertyName("signature")] public string Signature { get; set; }
    }
}
