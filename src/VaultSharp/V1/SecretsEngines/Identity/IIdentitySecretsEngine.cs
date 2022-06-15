using System.Threading.Tasks;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Identity.Models;

namespace VaultSharp.V1.SecretsEngines.Identity;

/// <summary>
///     Identity Secrets Engine.
/// </summary>
public interface IIdentitySecretsEngine
{
    /// <summary>
    ///     Use this endpoint to generate a signed ID (OIDC) token.
    /// </summary>
    /// <param name="roleName">
    ///     <para>[required]</para>
    ///     The name of the role against which to generate a signed ID token.
    /// </param>
    /// <param name="mountPoint">
    ///     <para>[optional]</para>
    ///     The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Identity" />
    ///     Provide a value only if you have customized the Azure mount point.
    /// </param>
    /// <param name="wrapTimeToLive">
    ///     <para>[optional]</para>
    ///     The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
    /// </param>
    /// <returns>
    ///     The secret with the <see cref="IdentityToken" /> as the data.
    /// </returns>
    Task<Secret<IdentityToken>> GetTokenAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

    /// <summary>
    ///     This endpoint can verify the authenticity and active state of a signed ID token.
    /// </summary>
    /// <param name="token">
    ///     <para>[required]</para>
    ///     A signed OIDC compliant ID token.
    /// </param>
    /// <param name="clientId">
    ///     <para>[optional]</para>
    ///     Specifying the client ID optimizes validation time
    /// </param>
    /// <param name="mountPoint">
    ///     <para>[optional]</para>
    ///     The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Identity" />
    ///     Provide a value only if you have customized the Azure mount point.
    /// </param>
    /// <param name="wrapTimeToLive">
    ///     <para>[optional]</para>
    ///     The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
    /// </param>
    /// <returns>
    ///     Indicates if the token is active.
    /// </returns>
    Task<Secret<bool>> IntrospectTokenAsync(string token, string clientId = null, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateOrUpdateEntityResponse>> CreateOrUpdateEntityById(CreateOrUpdateEntityByIdCommand entity,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateOrUpdateEntityResponse>> CreateOrUpdateEntityByName(CreateOrUpdateEntityByNameCommand entity,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ReadEntityByIdResponse>> ReadEntityById(string id, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ReadEntityByIdResponse>> ReadEntityByName(string name, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ListInfo>> ListEntitiesByName(string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateAliasResponse>> CreateEntityAlias(CreateAliasCommand alias,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ReadEntityAliasByIdResponse>> ReadEntityAliasById(string id, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateAliasResponse>> UpdateEntityAliasById(string id, CreateAliasCommand alias,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task DeleteEntityAliasById(string id,
        string mountPoint = null);

    Task<Secret<ListInfo>> ListEntityAliasesById(string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateGroupResponse>> CreateGroup(CreateGroupCommand group,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task UpdateGroupById(string id, CreateGroupCommand group,
        string mountPoint = null);

    // doesnt seem to work properly in changing group name -> use byId
    //Task UpdateGroupByName(string name, CreateGroupCommand group,
    //    string mountPoint = null);

    Task DeleteGroupById(string id,
        string mountPoint = null);

    Task DeleteGroupByName(string name,
        string mountPoint = null);

    Task<Secret<ReadGroupResponse>> ReadGroupById(string id, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ReadGroupResponse>> ReadGroupByName(string name, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ListInfo>> ListGroupsById(string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ListInfo>> ListGroupsByName(string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateAliasResponse>> CreateGroupAlias(CreateGroupAliasCommand alias,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<ReadGroupAliasByIdResponse>> ReadGroupAliasById(string id, string mountPoint = null,
        string wrapTimeToLive = null);

    Task<Secret<CreateAliasResponse>> UpdateGroupAliasById(string id, CreateGroupAliasCommand alias,
        string mountPoint = null,
        string wrapTimeToLive = null);

    Task DeleteGroupAliasById(string id,
        string mountPoint = null);

    Task<Secret<ListInfo>> ListGroupAliasesById(string mountPoint = null,
        string wrapTimeToLive = null);
}