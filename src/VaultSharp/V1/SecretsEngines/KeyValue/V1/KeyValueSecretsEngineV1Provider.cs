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

        public async Task<Secret<ListInfo>> GenericReadSecretLocationPathListAsync(string path, string mountPoint = "secret")
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/" + path.Trim('/'), HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path, string mountPoint = "secret")
        {
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/" + mountPoint.Trim('/') + "/" + path.Trim('/') + "?list=true", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}