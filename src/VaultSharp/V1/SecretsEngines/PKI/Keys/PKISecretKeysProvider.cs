using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI.Keys
{
    internal class PKISecretKeysProvider : IPKIKeys
    {
        private readonly Polymath _polymath;
        const string InternalPath = "keys/generate/internal";
        const string ExternalPath = "keys/generate/exported";

        public PKISecretKeysProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<KeysData>> ListKeys(string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<KeysData>>(pkiBackendMountPoint ?? 
                                                                               _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, 
                    "/keys/?list=true", HttpMethod.Get, 
                    null, 
                    wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<KeyData>> GenerateKey(KeyData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(data, "data");

            var generationPath = data.KeyGenerationType == KeyGenerationType.Internal ? InternalPath : ExternalPath;

            var result = await _polymath.MakeVaultApiRequest<Secret<KeyData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, generationPath, HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.KeyGenerationType = data.KeyGenerationType;
            return result;
        }

        public async Task<Secret<KeyData>> GetKey(string keyName, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            var result = await _polymath.MakeVaultApiRequest<Secret<KeyData>>(
                    pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI,
                    "/key/" + keyName, HttpMethod.Get, null, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<ImportKeyData>> ImportKey(ImportKeyData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(data, "data");

            var importPath = "keys/import";

            var result = await _polymath.MakeVaultApiRequest<Secret<ImportKeyData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, importPath, HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task DeleteKey(string keyName, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            await _polymath.MakeVaultApiRequest<Secret<KeysData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/key/" + keyName, HttpMethod.Delete, null, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}