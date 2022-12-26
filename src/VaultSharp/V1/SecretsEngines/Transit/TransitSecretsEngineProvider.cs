using System;
using System.Net.Http;
using System.Net.NetworkInformation;
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

        public async Task CreateEncryptionKeyAsync(string keyName, CreateKeyRequestOptions createKeyRequestOptions = null, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/'),
                HttpMethod.Post,
                createKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ImportEncryptionKeyAsync(string keyName, ImportKeyRequestOptions importKeyRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(importKeyRequestOptions, "importKeyRequestOptions");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/import",
                HttpMethod.Post,
                importKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ImportEncryptionKeyVersionAsync(string keyName, ImportKeyVersionRequestOptions importKeyVersionRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(importKeyVersionRequestOptions, "importKeyVersionRequestOptions");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/import-version",
                HttpMethod.Post,
                importKeyVersionRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TransitWrappingKeyModel>> ReadWrappingKeyAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<TransitWrappingKeyModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/wrapping_key", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<EncryptionKeyInfo>> ReadEncryptionKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionKeyInfo>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/'),
                HttpMethod.Get, wrapTimeToLive: wrapTimeToLive)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllEncryptionKeysAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEncryptionKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/'),
                HttpMethod.Delete)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UpdateEncryptionKeyConfigAsync(string keyName, UpdateKeyRequestOptions updateKeyRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(updateKeyRequestOptions, "updateKeyRequestOptions");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/config",
                HttpMethod.Post,
                updateKeyRequestOptions)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RotateEncryptionKeyAsync(string keyName, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/rotate",
                HttpMethod.Post)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ExportedKeyInfo>> ExportKeyAsync(TransitKeyCategory keyType,
    string keyName, string version = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<ExportedKeyInfo>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                "/export/" + Enum.GetName(typeof(TransitKeyCategory), keyType).Replace("_", "-") + "/" + keyName +
                (string.IsNullOrEmpty(version) ? "" : "/" + version),
                HttpMethod.Get, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
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

        public async Task<Secret<EncryptionResponse>> RewrapAsync(string keyName, RewrapRequestOptions rewrapRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(rewrapRequestOptions, "rewrapRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/rewrap/" + keyName.Trim('/'),
                HttpMethod.Post,
                rewrapRequestOptions, wrapTimeToLive: wrapTimeToLive)
                    .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DataKeyResponse>> GenerateDataKeyAsync(string keyName, DataKeyRequestOptions dataKeyRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<DataKeyResponse>>(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/datakey/" +  Enum.GetName(typeof(TransitDataKeyType), dataKeyRequestOptions.DataKeyType) + "/" + keyName.Trim('/'), HttpMethod.Post, dataKeyRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RandomBytesResponse>> GenerateRandomBytesAsync(RandomBytesRequestOptions randomOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(randomOptions, "randomOptions");

            return await _polymath.MakeVaultApiRequest<Secret<RandomBytesResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/random/" + Enum.GetName(typeof(RandomBytesSource), randomOptions.Source),
                    HttpMethod.Post,
                    randomOptions, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<HashResponse>> HashDataAsync(HashRequestOptions hashOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(hashOptions, "hashOptions");

            return await _polymath.MakeVaultApiRequest<Secret<HashResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/hash/" + Enum.GetName(typeof(TransitHashAlgorithm), hashOptions.Algorithm).ToLowerInvariant().Replace("_", "-"),
                    HttpMethod.Post,
                    hashOptions, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<HmacResponse>> GenerateHmacAsync(string keyName, HmacRequestOptions hmacOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(hmacOptions, "hmacOptions");

            return await _polymath.MakeVaultApiRequest<Secret<HmacResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/hmac/" + keyName + "/" + Enum.GetName(typeof(TransitHashAlgorithm), hmacOptions.Algorithm).ToLowerInvariant().Replace("_", "-"),
                    HttpMethod.Post, hmacOptions, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SigningResponse>> SignDataAsync(string keyName, SignRequestOptions signOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(signOptions, "signOptions");

            return await _polymath.MakeVaultApiRequest<Secret<SigningResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/sign/" + keyName.Trim('/') + "/" + Enum.GetName(typeof(TransitHashAlgorithm), signOptions.HashAlgorithm).ToLowerInvariant().Replace("_", "-"),
                    HttpMethod.Post,
                    signOptions, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<VerifyResponse>> VerifySignedDataAsync(string keyName, VerifyRequestOptions verifyOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(verifyOptions, "verifyOptions");

            return await _polymath.MakeVaultApiRequest<Secret<VerifyResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/verify/" + keyName.Trim('/') + "/" + Enum.GetName(typeof(TransitHashAlgorithm), verifyOptions.HashAlgorithm).ToLowerInvariant().Replace("_", "-"),
                    HttpMethod.Post, verifyOptions, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<BackupKeyResponse>> BackupKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");

            return await _polymath.MakeVaultApiRequest<Secret<BackupKeyResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/backup/" + keyName,
                    HttpMethod.Get, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RestoreKeyAsync(RestoreKeyRequestOptions restoreOptions, string keyName = null, string mountPoint = null)
        {
            Checker.NotNull(restoreOptions, "restoreOptions");

            await _polymath.MakeVaultApiRequest(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/restore" + (string.IsNullOrWhiteSpace(keyName) ? "" : "/" + keyName),
                    HttpMethod.Post,
                    restoreOptions)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task TrimKeyAsync(string keyName, TrimKeyRequestOptions trimKeyRequestOptions, string mountPoint = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(trimKeyRequestOptions, "trimKeyRequestOptions");

            await _polymath.MakeVaultApiRequest(
                mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit, "/keys/" + keyName.Trim('/') + "/trim",
                HttpMethod.Post,
                trimKeyRequestOptions)
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

        public async Task<Secret<CacheResponse>> ReadCacheConfigAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CacheResponse>>(
                    mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transit,
                    "/cache-config",
                    HttpMethod.Get, wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
