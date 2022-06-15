using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

/// <summary>
///     Represents an Entity
/// </summary>
public class CreateOrUpdateEntityByIdCommand
{
    /// <summary>
    ///     Name of the entity
    ///     entity-UUID
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     ID of the entity.If set, updates the corresponding existing entity.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     Metadata to be associated with the entity.
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, string>? MetaData { get; set; }

    /// <summary>
    ///     Policies to be tied to the entity.
    /// </summary>
    [JsonProperty("policies")]
    public List<string>? Policies { get; set; }

    /// <summary>
    ///     Whether the entity is disabled. Disabled entities' associated tokens cannot be used, but are not revoked.
    /// </summary>
    [JsonProperty("disabled")]
    public bool Disabled { get; set; }
}