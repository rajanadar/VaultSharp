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

        public async Task<Secret<SecretData>> ReadSecretAsync(string path, int? version = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<SecretData>>("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/') + (version != null ? ("?version=" + version) : ""), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task<Secret<SecretData<T>>> ReadSecretAsync<T>(string path, int? version = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<SecretData<T>>>("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/') + (version != null ? ("?version=" + version) : ""), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<FullSecretMetadata>> ReadSecretMetadataAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<FullSecretMetadata>>("v1/" + mountPoint.Trim('/') + "/metadata/" + path.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathsAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var suffixPath = string.IsNullOrWhiteSpace(path) ? string.Empty : "/" + path.Trim('/');
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/metadata" + suffixPath + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> WriteSecretAsync(string path, IDictionary<string, object> data, int? checkAndSet = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            var requestData = new Dictionary<string, object>
            {
                { "data", data }
            };

            if (checkAndSet != null)
            {
                requestData.Add("options", new { cas = checkAndSet.Value });
            }

            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task<Secret<T>> WriteSecretAsync<T>(string path, T data, int? checkAndSet = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            var requestData = new Dictionary<string, object>
            {
                { "data", data }
            };

            if (checkAndSet != null)
            {
                requestData.Add("options", new { cas = checkAndSet.Value });
            }

            return await _polymath.MakeVaultApiRequest<Secret<T>>("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretVersionsAsync(string path, IList<int> versions, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/delete/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UndeleteSecretVersionsAsync(string path, IList<int> versions, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/undelete/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DestroySecretAsync(string path, IList<int> versions, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/destroy/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteMetadataAsync(string path, string mountPoint = "secret")
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/metadata/" + path.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}