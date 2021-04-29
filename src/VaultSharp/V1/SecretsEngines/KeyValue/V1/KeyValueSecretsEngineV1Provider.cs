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

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path, string mountPoint = null, string wrapTimeToLive = null)
        {
            return await ReadSecretAsync<Dictionary<string, object>>(path, mountPoint, wrapTimeToLive);
        }
        
        public async Task<Secret<T>> ReadSecretAsync<T>(string path, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<T>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV1, "/" + path.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathsAsync(string path, string mountPoint = null, string wrapTimeToLive = null)
        {
            var suffixPath = string.IsNullOrWhiteSpace(path) ? string.Empty : path.Trim('/') + "/";
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV1, "/" + suffixPath + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> WriteSecretAsync(string path, IDictionary<string, object> values, string mountPoint = null)
        {
            return await WriteSecretAsync(path, new Dictionary<string, object>(values), mountPoint);
        }
        
        public async Task<Secret<T>> WriteSecretAsync<T>(string path, T values, string mountPoint = null)
        {
            Checker.NotNull(path, "path");

            return await _polymath.MakeVaultApiRequest<Secret<T>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV1, "/" + path.Trim('/'), HttpMethod.Post, values).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string path, string mountPoint = null)
        {
            Checker.NotNull(path, "path");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.KeyValueV1, "/" + path.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}