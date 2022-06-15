using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

/// <summary>
///     Represents an Entity
/// </summary>
public class CreateOrUpdateEntityResponse
{
    [JsonProperty("id")] public string? Id { get; set; }

    [JsonProperty("aliases")] public string? Aliases { get; set; }

    [JsonProperty("name")] public string? Name { get; set; }
}