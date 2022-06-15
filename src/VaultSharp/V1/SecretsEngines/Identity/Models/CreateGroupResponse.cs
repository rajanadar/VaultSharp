using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

/// <summary>
///     Represents an Entity
/// </summary>
public class CreateGroupResponse
{
    /// <summary>
    ///     Name of the group. If set (and ID is not set), updates the corresponding existing group.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     ID of the group. If set, updates the corresponding existing group.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }
}