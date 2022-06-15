using Newtonsoft.Json;

public class CreateGroupAliasCommand
{
    /// <summary>
    ///     Name of the alias.Name should be the identifier of the client in the authentication source.
    ///     If the alias belongs to GitHub, it should be the GitHub team name ("dev-team").
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Group ID to which this alias belongs to.
    /// </summary>
    [JsonProperty("canonical_id")]
    public string? CanonicalId { get; set; }

    /// <summary>
    ///     Accessor of the mount to which the alias should belong to.
    /// </summary>
    [JsonProperty("mount_accessor")]
    public string? MountAccessor { get; set; }

    /// <summary>
    ///     ID of the group alias.If set, updates the corresponding group alias.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }
}