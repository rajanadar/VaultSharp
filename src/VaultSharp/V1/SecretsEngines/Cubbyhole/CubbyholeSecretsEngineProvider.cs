using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Cubbyhole
{
    internal class CubbyholeSecretsEngineProvider : ICubbyholeSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.Cubbyhole), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.Cubbyhole;
            }
        }

        public CubbyholeSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string secretPath, string wrapTimeToLive = null)
        {
            Checker.NotNull(secretPath, "secretPath");
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(MountPoint, "/" + secretPath.TrimStart('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathsAsync(string folderPath, string wrapTimeToLive = null)
        {
            Checker.NotNull(folderPath, "folderPath");
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(MountPoint, "/" + folderPath.TrimStart('/') + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteSecretAsync(string secretPath, IDictionary<string, object> values)
        {
            Checker.NotNull(secretPath, "secretPath");
            await _polymath.MakeVaultApiRequest(MountPoint, "/" + secretPath.Trim('/'), HttpMethod.Post, values).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string secretPath)
        {
            Checker.NotNull(secretPath, "secretPath");
            await _polymath.MakeVaultApiRequest(MountPoint, "/" + secretPath.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}