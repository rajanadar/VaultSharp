using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.GoogleCloudKMS
{
    internal class GoogleCloudKMSSecretsEngineProvider : IGoogleCloudKMSSecretsEngine
    {
        private readonly Polymath _polymath;

        public GoogleCloudKMSSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<EncryptionResponse>> EncryptAsync(string keyName, EncryptRequestOptions encryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(encryptRequestOptions, "encryptRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<EncryptionResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloudKMS, "/encrypt/" + keyName.Trim('/'), HttpMethod.Post, encryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DecryptionResponse>> DecryptAsync(string keyName, DecryptRequestOptions decryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(decryptRequestOptions, "decryptRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<DecryptionResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloudKMS, "/decrypt/" + keyName.Trim('/'), HttpMethod.Post, decryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ReEncryptionResponse>> ReEncryptAsync(string keyName, ReEncryptRequestOptions reEncryptRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(reEncryptRequestOptions, "reEncryptRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<ReEncryptionResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloudKMS, "/reencrypt/" + keyName.Trim('/'), HttpMethod.Post, reEncryptRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SignatureResponse>> SignAsync(string keyName, SignatureOptions signatureOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(signatureOptions, "signatureOptions");

            return await _polymath.MakeVaultApiRequest<Secret<SignatureResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloudKMS, "/sign/" + keyName.Trim('/'), HttpMethod.Post, signatureOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<VerificationResponse>> VerifyAsync(string keyName, VerificationOptions verificationOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(keyName, "keyName");
            Checker.NotNull(verificationOptions, "verificationOptions");

            return await _polymath.MakeVaultApiRequest<Secret<VerificationResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.GoogleCloudKMS, "/verify/" + keyName.Trim('/'), HttpMethod.Post, verificationOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}