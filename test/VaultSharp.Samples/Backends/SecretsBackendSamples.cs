using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaultSharp.Core;

using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;
using VaultSharp.V1.SecretsEngines.Transit;
using Xunit;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunSecretsEngineSamples()
        {
            RunSecretsEngineSamplesForCubbyHole();
            RunSecretsEngineSamplesForKeyValue();
            RunSecretsEngineSamplesForTransit();
        }

        private static void RunSecretsEngineSamplesForCubbyHole()
        {
            var secretPath = "a/test-path";

            var value = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
            _authenticatedVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath, value).Wait();

            var read1 = _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath).Result;
            Assert.True(read1.Data.Count == 2);

            value.Add("key3", true);
            _authenticatedVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath, value).Wait();

            var read2 = _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath).Result;
            Assert.True(read2.Data.Count == 3);

            var value2 = new Dictionary<string, object> { { "key1", "val1" }, { "key2", 2 } };
            _authenticatedVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretPath + "2", value2).Wait();

            var paths = _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretPathsAsync("a/").Result;
            Assert.True(paths.Data.Keys.Count() == 2);

            _authenticatedVaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(secretPath).Wait();
            Assert.ThrowsAsync<VaultApiException>(() => _authenticatedVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretPath)).Wait();

            _authenticatedVaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(secretPath + "2").Wait();

        }

        private static void RunSecretsEngineSamplesForKeyValue()
        {
            // raja todo
            // 1.1.0 doesn't have kv1 by default.

            /*

            var path = Guid.NewGuid().ToString();

            var values = new Dictionary<string, object>
            {
                {"foo", "kv2"},
                {"foo2", 345 }
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(path, values).Wait();

            var paths1 = _authenticatedVaultClient.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("").Result;
            Assert.True(paths1.Data.Keys.Count() == 1);

            var kv1Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(path).Result;
            Assert.True(kv1Secret.Data.Count == 2);

            _authenticatedVaultClient.V1.Secrets.KeyValue.V1.DeleteSecretAsync(path).Wait();

            */

            // raja todo: check this out later
            // return;

            var path = Guid.NewGuid().ToString();

            // mount a new v2 kv
            var kv2SecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.KeyValueV2,
                Config = new Dictionary<string, object>
                {
                    {  "version", "2" }
                },
                Path = path
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(kv2SecretsEngine).Wait();

            // wait for a second for vault to upgrade all keys
            System.Threading.Thread.Sleep(1000);

            var values = new Dictionary<string, object>
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };

            var currentMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(path, values, mountPoint: kv2SecretsEngine.Path).Result;

            Assert.True(currentMetadata.Data.Version == 1);

            var kv2Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(kv2Secret.Data.Data.Count == 2);

            // TODO: Check this properly later to enable kv2.

            /*

            // var subkeys = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretSubkeysAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            // Assert.True(subkeys.Data.Subkeys.Count > 0);

            var paths2 = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(paths2.Data.Keys.Count() == 1);

            */

            var kv2metadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(kv2metadata.Data.CurrentVersion == 1);

            /*

            // patch

            Assert.ThrowsAsync<VaultApiException>(() => _authenticatedVaultClient.V1.Secrets.KeyValue.V2.PatchSecretAsync(Guid.NewGuid().ToString(), new PatchSecretDataRequest(), mountPoint: kv2SecretsEngine.Path)).Wait();

            var newData = new Dictionary<string, object>
            {
                { "k2", "newv2" },
                { "k3", "v3" }
            };

            PatchSecretDataRequest patchSecretDataRequest = new PatchSecretDataRequest()
            {
                Data = newData,
            };

            var newCurrentData = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.PatchSecretAsync(path,
                patchSecretDataRequest, mountPoint: kv2SecretsEngine.Path).Result;

            Assert.True(newCurrentData.Data.Version == 2);

            var kv2SecretNew = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: kv2SecretsEngine.Path).Result;
            Assert.True(kv2SecretNew.Data.Data.Count == 3);
            Assert.True((string)kv2SecretNew.Data.Data["k2"] == "newv2");

            */

            var writeCustomMetadataRequest = new CustomMetadataRequest
            {
                CustomMetadata = new Dictionary<string, string>
                {
                    { "owner", "system"},
                    { "expired_in", "20331010"}
                }
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretMetadataAsync(path, writeCustomMetadataRequest, mountPoint: kv2SecretsEngine.Path).Wait();

            var kv2metadata2 = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(path, mountPoint: kv2SecretsEngine.Path).Result;

            Assert.True(kv2metadata2.Data.CustomMetadata.Count == 2);
            Assert.True(kv2metadata2.Data.CustomMetadata["expired_in"] == "20331010");
            Assert.True(kv2metadata2.Data.CustomMetadata["owner"] == "system");

            var patchCustomMetadataRequest = new CustomMetadataRequest
            {
                CustomMetadata = new Dictionary<string, string>
                {
                    { "locale", "EN"},
                    { "expired_in", "20341010"}
                }
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.PatchSecretMetadataAsync(path, patchCustomMetadataRequest, mountPoint: kv2SecretsEngine.Path).Wait();

            var kv2metadata3 = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(path, mountPoint: kv2SecretsEngine.Path).Result;

            Assert.True(kv2metadata3.Data.CustomMetadata.Count == 3);
            Assert.True(kv2metadata3.Data.CustomMetadata["expired_in"] == "20341010");
            Assert.True(kv2metadata3.Data.CustomMetadata["owner"] == "system");
            Assert.True(kv2metadata3.Data.CustomMetadata["locale"] == "EN");

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.DestroySecretAsync(path, new List<int> { kv2metadata.Data.CurrentVersion }, mountPoint: kv2SecretsEngine.Path).Wait();

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(kv2SecretsEngine.Path).Wait();
        }

        private static void RunSecretsEngineSamplesForTransit()
        {
            var path = Guid.NewGuid().ToString();

            var transitSecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.Transit,
                Path = path
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(transitSecretsEngine).Wait();

            var keyName = Guid.NewGuid().ToString();
            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(keyName, new CreateKeyRequestOptions() { Exportable = true }, path).Wait();

            var context = "context1";
            var plainText = "raja";
            var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            var encodedContext = Convert.ToBase64String(Encoding.UTF8.GetBytes(context));

            var nonce = Convert.ToBase64String(Enumerable.Range(0, 12).Select(i => (byte)i).ToArray());
            var nonce2 = Convert.ToBase64String(Enumerable.Range(0, 12).Select(i => (byte)(i + 1)).ToArray());

            var encryptOptions = new EncryptRequestOptions
            {
                Base64EncodedPlainText = encodedPlainText,
                Base64EncodedContext = encodedContext,
                ConvergentEncryption = true,
                Nonce = nonce
            };

            var encryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions, path).Result;
            var cipherText = encryptionResponse.Data.CipherText;

            /*
            encryptOptions = new EncryptRequestOptions
            {
                BatchedEncryptionItems = new List<EncryptionItem>
                {
                    new EncryptionItem { KeyVersion = 1, Nonce = nonce, Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                    new EncryptionItem { KeyVersion = 1, Nonce = nonce, Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                    new EncryptionItem { KeyVersion = 1, Nonce = nonce, Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                }
            };

            var batchedEncryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions, path).Result;
            var firstCipherText = batchedEncryptionResponse.Data.BatchedResults.First().CipherText;

            var decryptOptions = new DecryptRequestOptions
            {
                CipherText = cipherText,
                Base64EncodedContext = encodedContext,
                Nonce = nonce
            };

            var decryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions, path).Result;
            var gotPlainText = decryptionResponse.Data.Base64EncodedPlainText;

            decryptOptions = new DecryptRequestOptions
            {
                BatchedDecryptionItems = new List<DecryptionItem>
                {
                    new DecryptionItem { Nonce = nonce, Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(0).CipherText },
                    new DecryptionItem { Nonce = nonce, Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(1).CipherText },
                    new DecryptionItem { Nonce = nonce, Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(2).CipherText },
                }
            };

            var batchedDecryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions, path).Result;
            var firstPlainText = batchedDecryptionResponse.Data.BatchedResults.First().Base64EncodedPlainText;

            */

            // Generate Data Key
            var dataKeyOptions = new DataKeyRequestOptions
            {
                Base64EncodedContext = encodedContext,
                Nonce = nonce
            };

            Secret<DataKeyResponse> dataKeyResponse = _authenticatedVaultClient.V1.Secrets.Transit.GenerateDataKeyAsync("plaintext", keyName, dataKeyOptions, path).Result;

            var encodedDataKeyPlainText = dataKeyResponse.Data.Base64EncodedPlainText;
            var dataKeyCipherText = dataKeyResponse.Data.Base64EncodedPlainText;

            // var trimOptions = new TrimKeyRequestOptions { MinimumAvailableVersion = 1 };
            // _authenticatedVaultClient.V1.Secrets.Transit.TrimKeyAsync(keyName, trimOptions).Wait();

            var exportedKey = _authenticatedVaultClient.V1.Secrets.Transit
                .ExportKeyAsync(TransitKeyCategory.encryption_key, keyName, version: null, path).Result;
            DisplayJson(exportedKey);

            exportedKey = _authenticatedVaultClient.V1.Secrets.Transit
                .ExportKeyAsync(TransitKeyCategory.encryption_key, keyName, version: "latest", path).Result;
            DisplayJson(exportedKey);


            var backupKey = new CreateKeyRequestOptions() { Exportable = true, AllowPlaintextBackup = true, Type = TransitKeyType.aes256_gcm96 };
            keyName = "backupKey";
            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(keyName, backupKey, path).Wait();

            var signKey = new CreateKeyRequestOptions() { Exportable = true, AllowPlaintextBackup = true, Type = TransitKeyType.ecdsa_p256 };
            var signKeyName = "signKey";
            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(signKeyName, signKey, path).Wait();

            var transit = _authenticatedVaultClient.V1.Secrets.Transit;

            var backup = transit.BackupKeyAsync(keyName, path).Result;
            DisplayJson(backup);

            var backupData = new RestoreKeyRequestOptions { BackupData = backup.Data.BackupData };
            transit.RestoreKeyAsync(keyName + "restored", backupData, path).Wait();

            var encodedText = Convert.ToBase64String(Encoding.UTF8.GetBytes("testplaintext"));
            var encryptOptions2 = new EncryptRequestOptions
            {
                Base64EncodedPlainText = encodedText,
            };
            var encrypted = transit.EncryptAsync(keyName, encryptOptions2, path).Result;
            DisplayJson(encrypted);

            var decryptOptions = new DecryptRequestOptions { CipherText = encrypted.Data.CipherText };
            var decrypted = transit.DecryptAsync(keyName + "restored", decryptOptions, path).Result;
            DisplayJson(decrypted);

            Assert.Equal(encodedText, decrypted.Data.Base64EncodedPlainText);

            // Random Gen
            var randomOpts = new RandomBytesRequestOptions { Format = OutputEncodingFormat.base64 };

            var base64Response = transit.GenerateRandomBytesAsync(64, randomOpts, path).Result;
            DisplayJson(base64Response);
            Assert.Equal(88, base64Response.Data.EncodedRandomBytes.Length);

            randomOpts.Format = OutputEncodingFormat.hex;
            var hexResponse = transit.GenerateRandomBytesAsync(64, randomOpts, path).Result;
            DisplayJson(hexResponse);
            Assert.Equal(128, hexResponse.Data.EncodedRandomBytes.Length);

            randomOpts.Source = RandomBytesSource.all;
            var allEntropySource = transit.GenerateRandomBytesAsync(64, randomOpts, path).Result;
            DisplayJson(allEntropySource);
            Assert.Equal(128, allEntropySource.Data.EncodedRandomBytes.Length);

            // Run Hmac
            var base64Input =
               Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the value we will use as plaintext here."));

            //Act 1 - Verify HMAC
            var hmacOptions = new HmacRequestOptions { Base64EncodedInput = base64Input };
            var hmacResponse = transit.GenerateHmacAsync(HashAlgorithm.sha2_256, keyName, hmacOptions, path).Result;
            DisplayJson(hmacResponse);

            var verifyOptions = new VerifyRequestOptions
            {
                Base64EncodedInput = base64Input,
                Hmac = hmacResponse.Data.Hmac,
                MarshalingAlgorithm = MarshalingAlgorithm.Asn1
            };
            var verifyResponse = transit.VerifySignedDataAsync(HashAlgorithm.sha2_256, keyName, verifyOptions, path).Result;
            DisplayJson(verifyResponse);

            Assert.True(verifyResponse.Data.Valid);

            // Run Signature
            var base64InputForSign =
                Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the value we will use as plaintext here."));
            var signOptions = new SignRequestOptions { Base64EncodedInput = base64InputForSign, MarshalingAlgorithm = MarshalingAlgorithm.Asn1 };
            var signResponse = transit.SignDataAsync(HashAlgorithm.sha2_256, signKeyName, signOptions, path).Result;
            DisplayJson(signResponse);

            var verifyOptionsForSign = new VerifyRequestOptions
            {
                Base64EncodedInput = base64Input,
                Signature = signResponse.Data.Signature,
                MarshalingAlgorithm = MarshalingAlgorithm.Asn1
            };
            var verifyResponseForSign = transit.VerifySignedDataAsync(HashAlgorithm.sha2_256, signKeyName, verifyOptionsForSign, path).Result;
            DisplayJson(verifyResponseForSign);

            //Assert
            Assert.True(verifyResponseForSign.Data.Valid);

            // Run Hash
            var hashOpts = new HashRequestOptions
            {
                Format = OutputEncodingFormat.base64,
                Base64EncodedInput = Convert.ToBase64String(Encoding.UTF8.GetBytes("Let's hash this"))
            };
            var hashResponse = transit.HashDataAsync(HashAlgorithm.sha2_256, hashOpts, path).Result;
            DisplayJson(hashResponse);

            // Run Cache
            var cacheResult = transit.ReadCacheConfigAsync(path).Result;
            DisplayJson(cacheResult);

            //Assert
            Assert.NotNull(cacheResult);
            Assert.True(0 == cacheResult.Data.Size || 10 <= cacheResult.Data.Size);

            //Act 2
            var newSize = cacheResult.Data.Size == 0 ? 25 : cacheResult.Data.Size + 1;
            var cacheOptions = new CacheConfigRequestOptions { Size = newSize };
            transit.SetCacheConfigAsync(cacheOptions, path).Wait();

            cacheOptions.Size -= 1;
            transit.SetCacheConfigAsync(cacheOptions, path).Wait();

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(path).Wait();
        }
    }
}
