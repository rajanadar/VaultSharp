using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KeyManagement
{
    internal class KeyManagementSecretsEngineProvider : IKeyManagementSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.KeyManagement), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.KeyManagement;
            }
        }

        public KeyManagementSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<KeyManagementKey>> ReadKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<KeyManagementKey>>(mountPoint ?? MountPoint, "/key/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<KeyManagementKMSKey>> ReadKeyInKMSAsync(string kmsName, string keyName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(kmsName, "kmsName");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<KeyManagementKMSKey>>(mountPoint ?? MountPoint, "/kms/" + kmsName.Trim('/') + "/key/" + keyName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}