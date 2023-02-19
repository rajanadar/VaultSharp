using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    internal class IdentitySecretsEngineProvider : IIdentitySecretsEngine
    {
        private readonly Polymath _polymath;

        public IdentitySecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<IdentityToken>> GetTokenAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<IdentityToken>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity, "/oidc/token/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<bool>> IntrospectTokenAsync(string token, string clientId = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(token, "token");

            return await _polymath.MakeVaultApiRequest<Secret<bool>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity, "/oidc/introspect", HttpMethod.Post, new { token, client_id = clientId }, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CreateEntityResponse>> CreateEntityAsync(CreateEntityRequest createEntityRequest, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(createEntityRequest, "createEntityRequest");

            return await _polymath.MakeVaultApiRequest<Secret<CreateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity",
                HttpMethod.Post,
                createEntityRequest,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Entity>> ReadEntityByIdAsync(string entityId, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(entityId, "entityId");

            return await _polymath.MakeVaultApiRequest<Secret<Entity>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id/" + entityId.Trim('/'),
                HttpMethod.Get,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UpdateEntityResponse>> UpdateEntityByIdAsync(UpdateEntityRequest updateEntityRequest, string entityId, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(entityId, "entityId");

            return await _polymath.MakeVaultApiRequest<Secret<UpdateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id/" + entityId.Trim('/'),
                HttpMethod.Post,
                updateEntityRequest,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEntityByIdAsync(string entityId, string mountPoint = null)
        {
            Checker.NotNull(entityId, "entityId");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id/" + entityId.Trim('/'),
                HttpMethod.Delete)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task BatchDeleteEntitiesByIdAsync(BatchDeleteEntitiesRequest batchDeleteEntitiesRequest, string mountPoint = null)
        {
            Checker.NotNull(batchDeleteEntitiesRequest, "batchDeleteEntitiesRequest");

            if (batchDeleteEntitiesRequest.EntityIds.Count > 0)
                await _polymath.MakeVaultApiRequest(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                    "/entity/batch-delete",
                    HttpMethod.Post,
                    batchDeleteEntitiesRequest)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListEntitiesResponse>> ListEntitiesByIdAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListEntitiesResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id", _polymath.ListHttpMethod,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CreateEntityResponse>> CreateEntityByNameAsync(CreateEntityByNameRequest createEntityByNameRequest, string name, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(name, "name");
            Checker.NotNull(createEntityByNameRequest, "createEntityByNameRequest");

            return await _polymath.MakeVaultApiRequest<Secret<CreateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Post,
                createEntityByNameRequest,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UpdateEntityResponse>> UpdateEntityByNameAsync(UpdateEntityByNameRequest updateEntityByNameRequest, string name, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(name, "name");
            Checker.NotNull(updateEntityByNameRequest, "updateEntityByNameRequest");

            return await _polymath.MakeVaultApiRequest<Secret<UpdateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Post,
                updateEntityByNameRequest,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Entity>> ReadEntityByNameAsync(string name, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(name, "name");

            return await _polymath.MakeVaultApiRequest<Secret<Entity>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Get,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEntityByNameAsync(string name, string mountPoint = null)
        {
            Checker.NotNull(name, "name");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Delete)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListEntitiesResponse>> ListEntitiesByNameAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListEntitiesResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name", _polymath.ListHttpMethod,
                wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task MergeEntitiesAsync(MergeEntitiesRequest mergeEntitiesRequest, string mountPoint = null)
        {
            Checker.NotNull(mergeEntitiesRequest, "mergeEntitiesRequest");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/merge",
                HttpMethod.Post,
                mergeEntitiesRequest)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}