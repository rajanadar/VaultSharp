using System.Linq;
using System;
using Xunit;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.TOTP;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunTOTPSecretsBackendSamples()
        {
            var mountPoint = Guid.NewGuid().ToString();

            var totpSecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.TOTP,
                Path = mountPoint
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(totpSecretsEngine).Wait();

            // Create key 1

            var keyName = Guid.NewGuid().ToString();
            var createKeyRequest = new TOTPCreateKeyRequest()
            {
                KeyGenerationOption = new TOTPVaultBasedKeyGeneration
                {
                    Exported = true,
                    KeySize = 32,
                    Issuer = "raja-issuer",
                    AccountName = "raja-account-name",
                    QRSize = 200,
                    Skew = 1
                },
                AccountName = "raja-account-name",
                Algorithm = "SHA512",
                Issuer = "raja-issuer"
            };

            var createKeyResponse = _authenticatedVaultClient.V1.Secrets.TOTP.CreateKeyAsync(keyName, createKeyRequest, mountPoint).Result;
            DisplayJson(createKeyResponse);
            Assert.NotNull(createKeyResponse.Data.Url);
            Assert.NotNull(createKeyResponse.Data.Barcode);

            // Read key 1

            var retrievedKey = _authenticatedVaultClient.V1.Secrets.TOTP.ReadKeyAsync(keyName, mountPoint).Result;
            DisplayJson(retrievedKey);
            Assert.Equal(createKeyRequest.AccountName, retrievedKey.Data.AccountName);

            // Create key 2

            var keyName2 = Guid.NewGuid().ToString();
            var createKeyRequest2 = new TOTPCreateKeyRequest()
            {
                KeyGenerationOption = new TOTPVaultBasedKeyGeneration
                {
                    Exported = true,
                    KeySize = 64,
                    Issuer = "raja-issuer2",
                    AccountName = "raja-account-name2",
                    QRSize = 200,
                    Skew = 0
                },
                AccountName = "raja-account-name2",
                Algorithm = "SHA256",
                Issuer = "raja-issuer2"

            };

            var createKeyResponse2 = _authenticatedVaultClient.V1.Secrets.TOTP.CreateKeyAsync(keyName2, createKeyRequest2, mountPoint).Result;
            DisplayJson(createKeyResponse2);
            Assert.NotNull(createKeyResponse2.Data.Url);
            Assert.NotNull(createKeyResponse2.Data.Barcode);

            // Read all keys

            var allKeys = _authenticatedVaultClient.V1.Secrets.TOTP.ReadAllKeysAsync(mountPoint).Result;
            DisplayJson(allKeys);
            Assert.True(allKeys.Data.Keys.Count() == 2);

            // Delete key 2

            _authenticatedVaultClient.V1.Secrets.TOTP.DeleteKeyAsync(keyName2, mountPoint).Wait();
            allKeys = _authenticatedVaultClient.V1.Secrets.TOTP.ReadAllKeysAsync(mountPoint).Result;
            DisplayJson(allKeys);
            Assert.True(allKeys.Data.Keys.Count() == 1);

            // generate code
            var generatedCode = _authenticatedVaultClient.V1.Secrets.TOTP.GetCodeAsync(keyName, mountPoint).Result;
            DisplayJson(generatedCode);
            Assert.NotNull(generatedCode.Data.Code);

            // validate code
            var validResponse = _authenticatedVaultClient.V1.Secrets.TOTP.ValidateCodeAsync(keyName, generatedCode.Data.Code, mountPoint).Result;
            DisplayJson(validResponse);
            Assert.True(validResponse.Data.Valid);

            var invalidResponse = _authenticatedVaultClient.V1.Secrets.TOTP.ValidateCodeAsync(keyName, generatedCode.Data.Code + "2", mountPoint).Result;
            DisplayJson(invalidResponse);
            Assert.False(invalidResponse.Data.Valid);

            // unmount

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(mountPoint).Wait();
        }
    }
}