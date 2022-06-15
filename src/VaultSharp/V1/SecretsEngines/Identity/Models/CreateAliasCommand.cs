using System.Collections.Generic;
using Newtonsoft.Json;

public class CreateAliasCommand
{
    /// <summary>
    ///     Name of the alias.Name should be the identifier of the client in the authentication source.
    ///     For example, if the alias belongs to userpass backend,
    ///     the name should be a valid username within userpass auth method.
    ///     If the alias belongs to GitHub, it should be the GitHub username.
    ///     If the alias belongs to an approle auth method, the name should be a valid RoleID.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Entity ID to which this alias belongs to.
    /// </summary>
    [JsonProperty("canonical_id")]
    public string? CanonicalId { get; set; }

    /// <summary>
    ///     Accessor of the mount to which the alias should belong to.
    /// </summary>
    [JsonProperty("mount_accessor")]
    public string? MountAccessor { get; set; }

    /// <summary>
    ///     ID of the entity alias.If set, updates the corresponding entity alias.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     A map of arbitrary string to string valued user-provided metadata meant to describe the alias.
    /// </summary>
    [JsonProperty("custom_metadata")]
    public Dictionary<string, string>? CustomMetaData { get; set; }
}