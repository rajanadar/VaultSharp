using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

public class ReadGroupResponse
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

    [JsonProperty("member_entity_ids")] public List<string>? MemberEntityIds { get; set; }

    [JsonProperty("member_group_ids")] public List<string>? MemberGroupIds { get; set; }

    [JsonProperty("modify_index")] public int ModifyIndex { get; set; }

    /// <summary>
    ///     Whether the entity is disabled. Disabled entities' associated tokens cannot be used, but are not revoked.
    /// </summary>
    [JsonProperty("parent_group_ids")]
    public List<string>? ParentGroupIds { get; set; }

    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("namespace_id")] public string? NamespaceId { get; set; }

    [JsonProperty("creation_time")] public string? CreationTime { get; set; }

    [JsonProperty("last_update_time")] public string? LastUpdateTime { get; set; }

    [JsonProperty("alias")] public ReadGroupAliasByIdResponse? Alias { get; set; }
}