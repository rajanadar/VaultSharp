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

        public async Task<Secret<SecretData>> ReadSecretAsync(string path, int? version = null, string mountPoint = SecretsEngineDefaultPaths.KeyValue, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<SecretData>>("v1/" + mountPoint.Trim('/') + "/data/" + path.Trim('/') + (version != null ? ("?version=" + version) : ""), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<FullSecretMetadata>> ReadSecretMetadataAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValue, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<FullSecretMetadata>>("v1/" + mountPoint.Trim('/') + "/metadata/" + path.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathListAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValue, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/metadata/" + path.Trim('/') + "?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}