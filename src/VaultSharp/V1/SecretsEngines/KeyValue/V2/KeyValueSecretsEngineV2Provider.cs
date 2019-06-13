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

            var suffixPath = string.IsNullOrWhiteSpace(path) ? string.Empty : path.Trim('/') + "/";
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/metadata/" + suffixPath + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteSecretAsync(string path, IDictionary<string, object> data, int? checkAndSet = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            await WriteSecretAsync<IDictionary<string, object>>(path, data, checkAndSet, mountPoint);
        }
        
        public async Task WriteSecretAsync<T>(string path, T data, int? checkAndSet = null, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
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

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DestroySecretAsync(string path, IList<int> versions, string mountPoint = SecretsEngineDefaultPaths.KeyValueV2)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");
            Checker.NotNull(versions, "versions");

            var requestData = new { versions = versions };

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/destroy/" + path.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}