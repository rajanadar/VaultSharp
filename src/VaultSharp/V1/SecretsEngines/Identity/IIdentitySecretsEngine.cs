using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Identity Secrets Engine.
    /// </summary>
    public interface IIdentitySecretsEngine
    {
        /// <summary>
        /// Use this endpoint to generate a signed ID (OIDC) token.
        /// </summary>
        /// <param name="roleName"><para>[required]</para>
        /// The name of the role against which to generate a signed ID token.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the Azure mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="IdentityToken" /> as the data.
        /// </returns>
        Task<Secret<IdentityToken>> GetTokenAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint can verify the authenticity and active state of a signed ID token.
        /// </summary>
        /// <param name="token"><para>[required]</para>
        /// A signed OIDC compliant ID token.
        /// </param>
        /// <param name="clientId"><para>[optional]</para>
        /// Specifying the client ID optimizes validation time
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the Azure mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// Indicates if the token is active.
        /// </returns>
        Task<Secret<bool>> IntrospectTokenAsync(string token, string clientId = null, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint creates or updates an Entity.
        /// </summary>
        /// <param cref="CreateEntityRequest" name="createEntityRequest">
        /// Request object to create or update an entity.
        /// </param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param> 
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="CreateEntityResponse"/> as the data
        /// </returns>
        Task<Secret<CreateEntityResponse>> CreateEntityAsync(CreateEntityRequest createEntityRequest, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint queries the entity by its identifier.
        /// </summary>
        /// <param name="entityId">Identifier of the entity.</param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="Entity"/> as the data
        /// </returns>
        Task<Secret<Entity>> ReadEntityByIdAsync(string entityId, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes all entities provided.
        /// </summary>
        /// <param cref="BatchDeleteEntitiesRequest" name="batchDeleteEntitiesRequest"></param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        Task BatchDeleteEntitiesByIdAsync(BatchDeleteEntitiesRequest batchDeleteEntitiesRequest, string mountPoint = null);

        /// <summary>
        /// This endpoint is used to create an entity by a given name.
        /// </summary>
        /// <param cref="CreateEntityByNameRequest" name="createEntityByNameRequest"></param>
        /// <param name="name">Name of the entity</param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <returns></returns>
        Task<Secret<CreateEntityResponse>> CreateEntityByNameAsync(CreateEntityByNameRequest createEntityByNameRequest, string name, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint deletes an entity and all its associated aliases.
        /// </summary>
        /// <param name="entityId">Identifier of the entity.</param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        Task DeleteEntityByIdAsync(string entityId, string mountPoint = null);

        /// <summary>
        /// This endpoint deletes an entity and all its associated aliases.
        /// </summary>
        /// <param name="name">Name of the entity.</param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        Task DeleteEntityByNameAsync(string name, string mountPoint = null);

        /// <summary>
        /// This endpoint returns a list of available entities by their 
        /// identifiers.
        /// </summary>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <returns>List of keys.</returns>
        Task<Secret<ListEntitiesResponse>> ListEntitiesByIdAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint returns a list of available entities by their 
        /// names.
        /// </summary>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <returns>List of keys.</returns>
        Task<Secret<ListEntitiesResponse>> ListEntitiesByNameAsync(string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint merges many entities into one entity. Additionally, 
        /// all groups associated with from_entity_ids are merged with those of 
        /// to_entity_id.
        /// </summary>
        /// <param cref="MergeEntitiesRequest" name="mergeEntitiesRequest"></param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        Task MergeEntitiesAsync(MergeEntitiesRequest mergeEntitiesRequest, string mountPoint = null);


        /// <summary>
        /// This endpoint queries the entity by its name.
        /// </summary>
        /// <param name="name">Name of the entity.</param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="Entity"/> as the data
        /// </returns>
        Task<Secret<Entity>> ReadEntityByNameAsync(string name, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint updates an Entity.
        /// </summary>
        /// <param cref="UpdateEntityRequest" name="updateEntityRequest">
        /// Request object to create or update an entity.
        /// </param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param> 
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <param name="entityId">Id of the entity to be updated</param>
        /// <returns>
        /// The secret with the <see cref="UpdateEntityResponse"/> as the data
        /// </returns>
        Task<Secret<UpdateEntityResponse>> UpdateEntityByIdAsync(UpdateEntityRequest updateEntityRequest, string entityId, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint updates an Entity by name.
        /// </summary>
        /// <param cref="UpdateEntityByNameRequest" name="updateEntityByNameRequest">
        /// Request object to create or update an entity.
        /// </param>
        /// <param name="mountPoint">
        /// <para>[optional]</para>
        /// The mount point for the backend. 
        /// Defaults to <see cref="SecretsEngineMountPoints.Identity" />
        /// Provide a value only if you have customized the mount point.
        /// </param> 
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of 
        /// seconds or a string duration of seconds.
        /// </param>
        /// <param name="name">Name of the entity to be updated.</param>
        /// <returns>
        /// The secret with the <see cref="UpdateEntityResponse"/> as the data
        /// </returns>
        Task<Secret<UpdateEntityResponse>> UpdateEntityByNameAsync(UpdateEntityByNameRequest updateEntityByNameRequest, string name, string mountPoint = null, string wrapTimeToLive = null);
    }
}