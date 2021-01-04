using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    internal class TransitSecretsEngineProvider : ITransitSecretsEngine
    {
        private readonly Polymath _polymath;

        public TransitSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<EncryptionResponse>> EncryptAsync(string keyName, EncryptRequestOptions encryptRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            return await _polymath.MakeVaultApiRequest<Secret<EncryptionResponse>>("v1/" + mountPoint.Trim('/') + "/encrypt/" + keyName.Trim('/'), HttpMethod.Post, encryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DecryptionResponse>> DecryptAsync(string keyName, DecryptRequestOptions decryptRequestOptions, string mountPoint = "transit", string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            return await _polymath.MakeVaultApiRequest<Secret<DecryptionResponse>>("v1/" + mountPoint.Trim('/') + "/decrypt/" + keyName.Trim('/'), HttpMethod.Post, decryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllEncryptionKeysAsync(string mountPoint = SecretsEngineDefaultPaths.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/keys?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DataKeyResponse>> GenerateDataKeyAsync(string keyType, string keyName, DataKeyRequestOptions dataKeyRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyType, "keyType");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<DataKeyResponse>>(
                "v1/" + mountPoint.Trim('/') + "/datakey/" + keyType.Trim('/')+ "/" + keyName.Trim('/'), HttpMethod.Post, dataKeyRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateEncryptionKeyAsync(string keyName, CreateKeyRequestOptions createKeyRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(createKeyRequestOptions, "createKeyRequestOptions");
            Checker.NotNull(mountPoint, "mountPoint");

            await _polymath.MakeVaultApiRequest<object>(
                "v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'),
                HttpMethod.Post,
                createKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<EncryptionKeyInfo>> ReadEncryptionKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.Transit)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionKeyInfo>>(
                "v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'),
                HttpMethod.Get)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UpdateEncryptionKeyConfigAsync(string keyName, UpdateKeyRequestOptions updateKeyRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(updateKeyRequestOptions, "updateKeyRequestOptions");
            Checker.NotNull(mountPoint, "mountPoint");

            await _polymath.MakeVaultApiRequest<object>(
                "v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/') + "/config",
                HttpMethod.Post,
                updateKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEncryptionKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.Transit)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(mountPoint, "mountPoint");

            await _polymath.MakeVaultApiRequest<object>(
                "v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/'),
                HttpMethod.Delete)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RotateEncryptionKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.Transit)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(mountPoint, "mountPoint");

            await _polymath.MakeVaultApiRequest<object>(
                "v1/" + mountPoint.Trim('/') + "/keys/" + keyName.Trim('/') + "/rotate",
                HttpMethod.Post)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<EncryptionResponse>> RewrapAsync(string keyName, RewrapRequestOptions rewrapRequestOptions, string mountPoint = SecretsEngineDefaultPaths.Transit)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(rewrapRequestOptions, "rewrapRequestOptions");
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionResponse>>(
                "v1/" + mountPoint.Trim('/') + "/rewrap/" + keyName.Trim('/'),
                HttpMethod.Post,
                rewrapRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}