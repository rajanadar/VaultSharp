using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V2
{
    internal class KeyValueSecretsEngineV2Provider : IKeyValueSecretsEngineV2
    {
        private readonly Polymath _polymath;

        public KeyValueSecretsEngineV2Provider(Polymath polymath)
        {
            _polymath = polymath;
        }        

        public async Task<Secret<SecretData>> ReadSecretAsync(string path, int? version = null, string mountPoint = null, string wrapTimeToLive = null)
        {            
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<SecretData>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/data/" + path.Trim('/') + (version != null ? ("?version=" + version) : ""), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task<Secret<SecretData<T>>> ReadSecretAsync<T>(string path, int? version = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<SecretData<T>>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/data/" + path.Trim('/') + (version != null ? ("?version=" + version) : ""), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<FullSecretMetadata>> ReadSecretMetadataAsync(string path, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<FullSecretMetadata>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/metadata/" + path.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathsAsync(string path, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/metadata/" + path.Trim('/') + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SecretSubkeysInfo>> ReadSecretSubkeysAsync(string path, int version = 0, int depth = 0, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(path, "path");

            string queryParameters = "?version=" + version + "&depth=" + depth;

            return await _polymath.MakeVaultApiRequest<Secret<SecretSubkeysInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/subkeys/" + path.Trim('/') + queryParameters, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CurrentSecretMetadata>> WriteSecretAsync<T>(string path, T data, int? checkAndSet = null, string mountPoint = null)
        {
            Checker.NotNull(path, "path");

            var requestData = new Dictionary<string, object>
            {
                { "data", data }
            };

            if (checkAndSet != null)
            {
                requestData.Add("options", new { cas = checkAndSet.Value });
            }

            return await _polymath.MakeVaultApiRequest<Secret<CurrentSecretMetadata>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/data/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CurrentSecretMetadata>> PatchSecretAsync(string path, PatchSecretDataRequest patchSecretDataRequest,  string mountPoint = null)
        {
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<CurrentSecretMetadata>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/data/" + path.Trim('/'), new HttpMethod("PATCH"), patchSecretDataRequest).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string path, string mountPoint = null)
        {
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/data/" + path.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretVersionsAsync(string path, IList<int> versions, string mountPoint = null)
        {
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/delete/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UndeleteSecretVersionsAsync(string path, IList<int> versions, string mountPoint = null)
        {
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/undelete/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DestroySecretAsync(string path, IList<int> versions, string mountPoint = null)
        {
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/destroy/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteMetadataAsync(string path, string mountPoint = null)
        {
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV2, "/metadata/" + path.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}