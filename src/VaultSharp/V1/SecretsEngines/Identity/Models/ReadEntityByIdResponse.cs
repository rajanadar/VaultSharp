using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

public class ReadEntityByIdResponse
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

    [JsonProperty("direct_group_ids")] public List<string>? DirectGroupIds { get; set; }

    [JsonProperty("group_ids")] public List<string>? GroupIds { get; set; }

    [JsonProperty("inherited_group_ids")] public List<string>? InheritedGroupIds { get; set; }

    /// <summary>
    ///     Whether the entity is disabled. Disabled entities' associated tokens cannot be used, but are not revoked.
    /// </summary>
    [JsonProperty("disabled")]
    public bool Disabled { get; set; }

    [JsonProperty("merged_entity_ids")] public string? MergedEntityIds { get; set; }

    [JsonProperty("namespace_id")] public string? NamespaceId { get; set; }

    [JsonProperty("creation_time")] public string? CreationTime { get; set; }

    [JsonProperty("last_update_time")] public string? LastUpdateTime { get; set; }

    [JsonProperty("aliases")] public List<ReadGroupAliasByIdResponse>? Aliases { get; set; }
}