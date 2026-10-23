using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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

        public async Task<Secret<IdentityToken>> GetTokenAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<IdentityToken>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity, "/oidc/token/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<bool>> IntrospectTokenAsync(string token, string clientId = null, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(token, "token");

            return await _polymath.MakeVaultApiRequest<Secret<bool>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity, "/oidc/introspect", HttpMethod.Post, new TokenClientIdRequest { Token = token, ClientId = clientId }, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CreateEntityResponse>> CreateEntityAsync(CreateEntityRequest createEntityRequest, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(createEntityRequest, "createEntityRequest");

            return await _polymath.MakeVaultApiRequest<Secret<CreateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity",
                HttpMethod.Post,
                createEntityRequest,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Entity>> ReadEntityByIdAsync(string entityId, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(entityId, "entityId");

            return await _polymath.MakeVaultApiRequest<Secret<Entity>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id/" + entityId.Trim('/'),
                HttpMethod.Get,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UpdateEntityResponse>> UpdateEntityByIdAsync(UpdateEntityRequest updateEntityRequest, string entityId, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(entityId, "entityId");

            return await _polymath.MakeVaultApiRequest<Secret<UpdateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id/" + entityId.Trim('/'),
                HttpMethod.Post,
                updateEntityRequest,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEntityByIdAsync(string entityId, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(entityId, "entityId");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id/" + entityId.Trim('/'),
                HttpMethod.Delete, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task BatchDeleteEntitiesByIdAsync(BatchDeleteEntitiesRequest batchDeleteEntitiesRequest, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(batchDeleteEntitiesRequest, "batchDeleteEntitiesRequest");

            if (batchDeleteEntitiesRequest.EntityIds.Count > 0)
                await _polymath.MakeVaultApiRequest(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                    "/entity/batch-delete",
                    HttpMethod.Post,
                    batchDeleteEntitiesRequest, cancellationToken: cancellationToken)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListEntitiesResponse>> ListEntitiesByIdAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListEntitiesResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/id", _polymath.ListHttpMethod,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CreateEntityResponse>> CreateEntityByNameAsync(CreateEntityByNameRequest createEntityByNameRequest, string name, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(name, "name");
            Checker.NotNull(createEntityByNameRequest, "createEntityByNameRequest");

            return await _polymath.MakeVaultApiRequest<Secret<CreateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Post,
                createEntityByNameRequest,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UpdateEntityResponse>> UpdateEntityByNameAsync(UpdateEntityByNameRequest updateEntityByNameRequest, string name, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(name, "name");
            Checker.NotNull(updateEntityByNameRequest, "updateEntityByNameRequest");

            return await _polymath.MakeVaultApiRequest<Secret<UpdateEntityResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Post,
                updateEntityByNameRequest,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Entity>> ReadEntityByNameAsync(string name, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(name, "name");

            return await _polymath.MakeVaultApiRequest<Secret<Entity>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Get,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEntityByNameAsync(string name, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(name, "name");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name/" + name.Trim('/'),
                HttpMethod.Delete, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListEntitiesResponse>> ListEntitiesByNameAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListEntitiesResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/name", _polymath.ListHttpMethod,
                wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task MergeEntitiesAsync(MergeEntitiesRequest mergeEntitiesRequest, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(mergeEntitiesRequest, "mergeEntitiesRequest");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/entity/merge",
                HttpMethod.Post,
                mergeEntitiesRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateNamedKeyAsync(string keyName, CreateNamedKeyRequest createNamedKeyRequest, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(createNamedKeyRequest, "createNamedKeyRequest");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/oidc/key/" + keyName.Trim('/'), 
                HttpMethod.Post, 
                createNamedKeyRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<ReadNamedKeyResponse> ReadNamedKeyAsync(string keyName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<ReadNamedKeyResponse>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/oidc/key/" + keyName.Trim('/'),
                HttpMethod.Get, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateRoleAsync(string roleName, CreateRoleRequest createRoleRequest, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(createRoleRequest, "createRoleRequest");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/oidc/role/" + roleName.Trim('/'),
                HttpMethod.Post,
                createRoleRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<ReadRoleResponse> ReadRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<ReadRoleResponse>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/oidc/role/" + roleName.Trim('/'),
                HttpMethod.Get, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/oidc/role/" + roleName.Trim('/'),
                HttpMethod.Delete, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteNamedKeyAsync(string keyName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Identity,
                "/oidc/key/" + keyName.Trim('/'),
                HttpMethod.Delete, cancellationToken: cancellationToken)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}