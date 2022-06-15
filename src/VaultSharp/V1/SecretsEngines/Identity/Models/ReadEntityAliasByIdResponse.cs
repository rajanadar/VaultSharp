using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

public class ReadEntityAliasByIdResponse
{
    /// <summary>
    ///     Name of the entity
    ///     entity-UUID
    /// </summary>
    [JsonProperty("canonical_id")]
    public string? CanonicalId { get; set; }

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
    [JsonProperty("mount_accessor")]
    public string? MountAccessor { get; set; }

    [JsonProperty("mount_path")] public string? MountPath { get; set; }

    [JsonProperty("mount_type")] public string? MountType { get; set; }

    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("namespace_id")] public string? NamespaceId { get; set; }

    [JsonProperty("local")] public bool Local { get; set; }

    [JsonProperty("merged_from_canonical_ids")]
    public string? MergedFromCanonicalIds { get; set; }

    [JsonProperty("creation_time")] public string? CreationTime { get; set; }

    [JsonProperty("last_update_time")] public string? LastUpdateTime { get; set; }
}