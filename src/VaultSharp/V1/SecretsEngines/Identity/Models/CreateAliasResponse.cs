using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

/// <summary>
///     Represents an Entity
/// </summary>
public class CreateAliasResponse
{
    /// <summary>
    ///     Entity ID to which this alias belongs to.
    /// </summary>
    [JsonProperty("canonical_id")]
    public string? CanonicalId { get; set; }

    /// <summary>
    ///     ID of the entity alias.If set, updates the corresponding entity alias.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }
}