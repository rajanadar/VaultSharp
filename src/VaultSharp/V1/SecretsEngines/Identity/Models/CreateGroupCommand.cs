using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity.Models;

public class CreateGroupCommand
{
    /// <summary>
    ///     Name of the group. If set (and ID is not set), updates the corresponding existing group.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Type of the group, internal or external.Defaults to internal.
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; } = "internal";

    /// <summary>
    ///     Accessor of the mount to which the alias should belong to.
    /// </summary>
    [JsonProperty("mount_accessor")]
    public string? MountAccessor { get; set; }

    /// <summary>
    ///     ID of the group. If set, updates the corresponding existing group.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     Metadata to be associated with the group.
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     Policies to be tied to the group.
    /// </summary>
    [JsonProperty("policies")]
    public List<string>? Policies { get; set; }

    /// <summary>
    ///     Group IDs to be assigned as group members.
    /// </summary>
    [JsonProperty("member_group_ids")]
    public List<string>? MemberGroupIds { get; set; }

    /// <summary>
    ///     Entity IDs to be assigned as group members.
    /// </summary>
    [JsonProperty("member_entity_ids")]
    public List<string>? MemberEntityIds { get; set; }
}