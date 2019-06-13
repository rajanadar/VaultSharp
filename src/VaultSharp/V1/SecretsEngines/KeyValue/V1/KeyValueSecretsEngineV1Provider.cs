using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V1
{
    internal class KeyValueSecretsEngineV1Provider : IKeyValueSecretsEngineV1
    {
        private readonly Polymath _polymath;

        public KeyValueSecretsEngineV1Provider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1, string wrapTimeToLive = null)
        {
            return await ReadSecretAsync<Dictionary<string, object>>(path, mountPoint, wrapTimeToLive);
        }
        
        public async Task<Secret<T>> ReadSecretAsync<T>(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<T>>("v1/" + mountPoint.Trim('/') + "/" + path.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathsAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            var suffixPath = string.IsNullOrWhiteSpace(path) ? string.Empty : path.Trim('/') + "/";
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/" + suffixPath + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteSecretAsync(string path, IDictionary<string, object> values, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1)
        {
            await WriteSecretAsync(path, values, mountPoint);
        }
        
        public async Task WriteSecretAsync<T>(string path, T values, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/" + path.Trim('/'), HttpMethod.Post, values).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValueV1)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/" + path.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}