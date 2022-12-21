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

        public async Task<Secret<EncryptionResponse>> EncryptAsync(string keyName, EncryptRequestOptions encryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            return await _polymath.MakeVaultApiRequest<Secret<EncryptionResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/encrypt/" + keyName.Trim('/'), HttpMethod.Post, encryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DecryptionResponse>> DecryptAsync(string keyName, DecryptRequestOptions decryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            return await _polymath.MakeVaultApiRequest<Secret<DecryptionResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/decrypt/" + keyName.Trim('/'), HttpMethod.Post, decryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllEncryptionKeysAsync(string mountPoint = null, string wrapTimeToLive = null)
        {

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DataKeyResponse>> GenerateDataKeyAsync(string keyType, string keyName, DataKeyRequestOptions dataKeyRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyType, "keyType");
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<DataKeyResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/datakey/" + keyType.Trim('/')+ "/" + keyName.Trim('/'), HttpMethod.Post, dataKeyRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateEncryptionKeyAsync(string keyName, CreateKeyRequestOptions createKeyRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(createKeyRequestOptions, "createKeyRequestOptions");

            await _polymath.MakeVaultApiRequest<object>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/'),
                HttpMethod.Post,
                createKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<EncryptionKeyInfo>> ReadEncryptionKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionKeyInfo>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/'),
                HttpMethod.Get)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UpdateEncryptionKeyConfigAsync(string keyName, UpdateKeyRequestOptions updateKeyRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(updateKeyRequestOptions, "updateKeyRequestOptions");

            await _polymath.MakeVaultApiRequest<object>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/config",
                HttpMethod.Post,
                updateKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEncryptionKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest<object>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/'),
                HttpMethod.Delete)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RotateEncryptionKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest<object>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/rotate",
                HttpMethod.Post)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<EncryptionResponse>> RewrapAsync(string keyName, RewrapRequestOptions rewrapRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(rewrapRequestOptions, "rewrapRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/rewrap/" + keyName.Trim('/'),
                HttpMethod.Post,
                rewrapRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task TrimKeyAsync(string keyName, TrimKeyRequestOptions trimKeyRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(trimKeyRequestOptions, "trimKeyRequestOptions");

            await _polymath.MakeVaultApiRequest<object>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/trim",
                HttpMethod.Post,
                trimKeyRequestOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ExportedKeyInfo>> ExportKeyAsync(TransitKeyCategory keyType,
            string keyName, string version = null, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<ExportedKeyInfo>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                "/export/" + keyType.ToString().Replace("_", "-") + "/" + keyName +
                (string.IsNullOrEmpty(version) ? "" : "/" + version),
                HttpMethod.Get)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

        }

        public async Task<Secret<BackupKeyResponse>> BackupKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            return await _polymath.MakeVaultApiRequest<Secret<BackupKeyResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/backup/" + keyName,
                    HttpMethod.Get)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RestoreKeyAsync(string keyName, RestoreKeyRequestOptions backupData, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(backupData, "backupData");

            await _polymath.MakeVaultApiRequest(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/restore/" + keyName,
                    HttpMethod.Post,
                    backupData)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RandomBytesResponse>> GenerateRandomBytesAsync(uint numberBytes, RandomBytesRequestOptions randomOptions, string mountPoint = null)
        {
            Checker.NotNull(randomOptions, "randomOptions");

            return await _polymath.MakeVaultApiRequest<Secret<RandomBytesResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/random/" + numberBytes,
                    HttpMethod.Post,
                    randomOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<HashResponse>> HashDataAsync(HashAlgorithm algorithm, HashRequestOptions hashOptions, string mountPoint = null)
        {
            Checker.NotNull(hashOptions, "hashOptions");
            var algo = AlgorithmToString(algorithm);
            if (algo != "")
                algo = "/" + algo;
            return await _polymath.MakeVaultApiRequest<Secret<HashResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/hash" + algo,
                    HttpMethod.Post,
                    hashOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<HmacResponse>> GenerateHmacAsync(HashAlgorithm algorithm, string keyName, HmacRequestOptions hmacOptions,
            string mountPoint = null)
        {
            Checker.NotNull(hmacOptions, "hmacOptions");
            var algo = AlgorithmToString(algorithm);
            if (algo != "")
                algo = "/" + algo;
            return await _polymath.MakeVaultApiRequest<Secret<HmacResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/hmac/" + keyName + algo,
                    HttpMethod.Post,
                    hmacOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SigningResponse>> SignDataAsync(HashAlgorithm algorithm, string keyName, SignRequestOptions signOptions, string mountPoint = null)
        {
            Checker.NotNull(signOptions, "signOptions");
            var algo = AlgorithmToString(algorithm);
            if (algo != "")
                algo = "/" + algo;
            return await _polymath.MakeVaultApiRequest<Secret<SigningResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/sign/" + keyName + algo,
                    HttpMethod.Post,
                    signOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

        }

        public async Task<Secret<VerifyResponse>> VerifySignedDataAsync(HashAlgorithm algorithm, string keyName, VerifyRequestOptions verifyOptions, string mountPoint = null)
        {
            Checker.NotNull(verifyOptions, "verifyOptions");
            var algo = AlgorithmToString(algorithm);
            if (algo != "")
                algo = "/" + algo;
            return await _polymath.MakeVaultApiRequest<Secret<VerifyResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/verify/" + keyName + algo,
                    HttpMethod.Post,
                    verifyOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CacheResponse>> ReadCacheConfigAsync(string mountPoint = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CacheResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/cache-config",
                    HttpMethod.Get)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task SetCacheConfigAsync(CacheConfigRequestOptions cacheOptions, string mountPoint = null)
        {
            Checker.NotNull(cacheOptions, "cacheOptions");
            await _polymath.MakeVaultApiRequest(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/cache-config",
                    HttpMethod.Post,
                    cacheOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        private string AlgorithmToString(HashAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case HashAlgorithm.sha2_224: return "sha2-224";
                case HashAlgorithm.sha2_384: return "sha2-384";
                case HashAlgorithm.sha2_256: return "sha2-256";
                case HashAlgorithm.sha2_512: return "sha2-512";
                case HashAlgorithm.sha3_224: return "sha3-224";
                case HashAlgorithm.sha3_384: return "sha3-384";
                case HashAlgorithm.sha3_256: return "sha3-256";
                case HashAlgorithm.sha3_512: return "sha3-512";
                case HashAlgorithm.Default:
                default:
                    return "";
            }
        }
    }
}
