using System.Linq;
using System.Text;
using System;
using Xunit;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.Transit;
using VaultSharp.V1.Commons;
using System.Collections.Generic;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunTransitSecretsBackendSamples()
        {
            var mountPoint = Guid.NewGuid().ToString();

            var transitSecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.Transit,
                Path = mountPoint
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(transitSecretsEngine).Wait();

            var keyName = Guid.NewGuid().ToString();
            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(keyName, new CreateKeyRequestOptions() 
            {
                Type = TransitKeyType.chacha20_poly1305,
                Exportable = false,
                
            }, mountPoint).Wait();

            var wrappingKey = _authenticatedVaultClient.V1.Secrets.Transit.ReadWrappingKeyAsync(mountPoint).Result;
            DisplayJson(wrappingKey);
            Assert.NotNull(wrappingKey.Data.PublicKey);

            var retrievedKey = _authenticatedVaultClient.V1.Secrets.Transit.ReadEncryptionKeyAsync(keyName, mountPoint).Result;
            DisplayJson(retrievedKey);
            Assert.Equal(TransitKeyType.chacha20_poly1305, retrievedKey.Data.Type);

            // raja todo: Import key and import key version apis.

            var secondConvergentKeyName = Guid.NewGuid().ToString();
            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(secondConvergentKeyName, new CreateKeyRequestOptions()
            {
                Type = TransitKeyType.aes256_gcm96,
                ConvergentEncryption = true,
                Derived = true
            }, mountPoint).Wait();

            var allKeys = _authenticatedVaultClient.V1.Secrets.Transit.ReadAllEncryptionKeysAsync(mountPoint).Result;
            DisplayJson(allKeys);
            Assert.True(allKeys.Data.Keys.Count() == 3); // there is an auto /import key.

            _authenticatedVaultClient.V1.Secrets.Transit.UpdateEncryptionKeyConfigAsync(keyName,
                new UpdateKeyRequestOptions
                {
                    Exportable = true
                }, mountPoint).Wait();

            var retrievedKey2 = _authenticatedVaultClient.V1.Secrets.Transit.ReadEncryptionKeyAsync(keyName, mountPoint).Result;
            DisplayJson(retrievedKey2);
            Assert.True(retrievedKey2.Data.Exportable);

            var exportFirstKey = _authenticatedVaultClient.V1.Secrets.Transit.ExportKeyAsync(TransitKeyCategory.encryption_key, keyName, mountPoint: mountPoint).Result;
            DisplayJson(exportFirstKey);
            Assert.True(exportFirstKey.Data.Keys.Count > 0);

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
                // Nonce = nonce  // Nonce cannot be provided anymore. "provided nonce not allowed for this key" error
            };

            var encryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions, mountPoint).Result;
            DisplayJson(encryptionResponse);
            Assert.NotNull(encryptionResponse.Data.CipherText);

            var cipherText = encryptionResponse.Data.CipherText;

            var decryptOptions = new DecryptRequestOptions
            {
                CipherText = cipherText,
                Base64EncodedContext = encodedContext,
                // Nonce = nonce
            };

            var decryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, decryptOptions, mountPoint).Result;
            DisplayJson(decryptionResponse);
            Assert.Equal(encodedPlainText, decryptionResponse.Data.Base64EncodedPlainText);

            // Nonce not allowed.
            var batchEncryptOptions = new EncryptRequestOptions
            {
                BatchedEncryptionItems = new List<EncryptionItem>
                {
                    new EncryptionItem { KeyVersion = 1, Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                    new EncryptionItem { KeyVersion = 1, Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                    new EncryptionItem { KeyVersion = 1, Base64EncodedContext = encodedContext, Base64EncodedPlainText = encodedPlainText },
                }
            };

            var batchedEncryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, batchEncryptOptions, mountPoint).Result;
            DisplayJson(batchedEncryptionResponse);
            Assert.True(batchedEncryptionResponse.Data.BatchedResults.Count == 3);

            var batchDecryptOptions = new DecryptRequestOptions
            {
                BatchedDecryptionItems = new List<DecryptionItem>
                {
                    new DecryptionItem { Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(0).CipherText },
                    new DecryptionItem { Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(1).CipherText },
                    new DecryptionItem { Base64EncodedContext = encodedContext, CipherText = batchedEncryptionResponse.Data.BatchedResults.ElementAt(2).CipherText },
                }
            };

            var batchedDecryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.DecryptAsync(keyName, batchDecryptOptions, mountPoint).Result;
            DisplayJson(batchedDecryptionResponse);
            Assert.True(batchedDecryptionResponse.Data.BatchedResults.Count == 3);

            _authenticatedVaultClient.V1.Secrets.Transit.RotateEncryptionKeyAsync(keyName, mountPoint).Wait();
            var retrievedRotatedKey = _authenticatedVaultClient.V1.Secrets.Transit.ReadEncryptionKeyAsync(keyName, mountPoint).Result;
            DisplayJson(retrievedRotatedKey);
            Assert.False(retrievedKey.Data.LatestVersion == 2);

            // test rewrap with direct encrypt.
            var rewrappedResult = _authenticatedVaultClient.V1.Secrets.Transit.RewrapAsync(keyName,
                new RewrapRequestOptions
                {
                    Base64EncodedContext = encodedContext,
                    CipherText = encryptionResponse.Data.CipherText,
                    // Nonce = nonce,
                }, mountPoint).Result;
            DisplayJson(rewrappedResult);
            Assert.NotNull(rewrappedResult.Data.CipherText);
            var directEncryptOptions = new EncryptRequestOptions
            {
                Base64EncodedPlainText = encodedPlainText,
                Base64EncodedContext = encodedContext,
                // Nonce = nonce
            };

            var directEncryptionResponse = _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, directEncryptOptions, mountPoint).Result;
            DisplayJson(directEncryptionResponse);
            // Assert.Equal(rewrappedResult.Data.CipherText, directEncryptionResponse.Data.CipherText);

            var backupKeyName = "backup-key";
            var backupKey = new CreateKeyRequestOptions() { Exportable = true, AllowPlaintextBackup = true, Type = TransitKeyType.ed25519 };

            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(backupKeyName, backupKey, mountPoint).Wait();

            var backupKeyResponse = _authenticatedVaultClient.V1.Secrets.Transit.BackupKeyAsync(backupKeyName, mountPoint).Result;
            DisplayJson(backupKeyResponse);
            Assert.NotNull(backupKeyResponse.Data.BackupData);

            var restoreKeyRequest = new RestoreKeyRequestOptions { BackupData = backupKeyResponse.Data.BackupData };
            _authenticatedVaultClient.V1.Secrets.Transit.RestoreKeyAsync(restoreKeyRequest, backupKeyName + "-restored", mountPoint).Wait();

            var signKeyName = "sign-key";
            var signKey = new CreateKeyRequestOptions() { Exportable = true, AllowPlaintextBackup = true, Type = TransitKeyType.ecdsa_p256 };

            _authenticatedVaultClient.V1.Secrets.Transit.CreateEncryptionKeyAsync(signKeyName, signKey, mountPoint).Wait();

            // Run Signature
            var base64InputForSign =
                Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the value we will use as plaintext here."));
            var signOptions = new SignRequestOptions { HashAlgorithm = TransitHashAlgorithm.SHA3_384, Base64EncodedInput = base64InputForSign, MarshalingAlgorithm = MarshalingAlgorithm.jws };
            var signResponse = _authenticatedVaultClient.V1.Secrets.Transit.SignDataAsync(signKeyName, signOptions, mountPoint).Result;
            DisplayJson(signResponse);
            Assert.NotNull(signResponse.Data.Signature);

            var verifyOptionsForSign = new VerifyRequestOptions
            {
                HashAlgorithm = TransitHashAlgorithm.SHA3_384,
                Base64EncodedInput = base64InputForSign,
                Signature = signResponse.Data.Signature,
                MarshalingAlgorithm = MarshalingAlgorithm.jws
            };
            var verifyResponseForSign = _authenticatedVaultClient.V1.Secrets.Transit.VerifySignedDataAsync(signKeyName, verifyOptionsForSign, mountPoint).Result;
            DisplayJson(verifyResponseForSign);
            Assert.True(verifyResponseForSign.Data.Valid);

            // Random Gen
            var byteLength  = 512;
            var randomOpts = new RandomBytesRequestOptions { BytesToGenerate = byteLength, Format = OutputEncodingFormat.base64 };

            var randomBytesResponse = _authenticatedVaultClient.V1.Secrets.Transit.GenerateRandomBytesAsync(randomOpts, mountPoint).Result;
            DisplayJson(randomBytesResponse);
            Assert.True(byteLength <= randomBytesResponse.Data.EncodedRandomBytes.Length);

            randomOpts.Format = OutputEncodingFormat.hex;
            var hexResponse = _authenticatedVaultClient.V1.Secrets.Transit.GenerateRandomBytesAsync(randomOpts, mountPoint).Result;
            DisplayJson(hexResponse);
            Assert.Equal(byteLength * 2, hexResponse.Data.EncodedRandomBytes.Length);

            randomOpts.Source = RandomBytesSource.all;
            var allEntropySource = _authenticatedVaultClient.V1.Secrets.Transit.GenerateRandomBytesAsync(randomOpts, mountPoint).Result;
            DisplayJson(allEntropySource);
            Assert.Equal(byteLength * 2, allEntropySource.Data.EncodedRandomBytes.Length);

            // Run Hmac
            var base64InputForHmac =
               Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the value we will use as plaintext here for hmac."));

            var hmacOptions = new HmacRequestOptions { Algorithm = TransitHashAlgorithm.SHA3_384, Base64EncodedInput = base64InputForHmac };

            var hmacResponse = _authenticatedVaultClient.V1.Secrets.Transit.GenerateHmacAsync(keyName, hmacOptions, mountPoint).Result;
            DisplayJson(hmacResponse);
            Assert.NotNull(hmacResponse.Data.Hmac);

            var verifyHmacOptions = new VerifyRequestOptions
            {
                HashAlgorithm = TransitHashAlgorithm.SHA3_384,
                Base64EncodedInput = base64InputForHmac,
                Hmac = hmacResponse.Data.Hmac,      
                MarshalingAlgorithm = MarshalingAlgorithm.asn1
            };

            var verifyHmacResponse = _authenticatedVaultClient.V1.Secrets.Transit.VerifySignedDataAsync(keyName, verifyHmacOptions, mountPoint).Result;
            DisplayJson(verifyHmacResponse);
            Assert.True(verifyHmacResponse.Data.Valid);

            // Run Hash
            var hashOpts = new HashRequestOptions
            {
                Algorithm = TransitHashAlgorithm.SHA3_384,
                Format = OutputEncodingFormat.base64,
                Base64EncodedInput = Convert.ToBase64String(Encoding.UTF8.GetBytes("Let's hash this"))
            };

            var hashResponse = _authenticatedVaultClient.V1.Secrets.Transit.HashDataAsync(hashOpts, mountPoint).Result;
            DisplayJson(hashResponse);
            Assert.NotNull(hashResponse.Data.HashSum);

            _authenticatedVaultClient.V1.Secrets.Transit.UpdateEncryptionKeyConfigAsync(keyName,
               new UpdateKeyRequestOptions
               {
                   MinimumEncryptionVersion = 2,
                   MinimumDecryptionVersion = 2
               }, mountPoint).Wait();

            var trimOptions = new TrimKeyRequestOptions
            {
                MinimumAvailableVersion = 2
            };
            
            _authenticatedVaultClient.V1.Secrets.Transit.TrimKeyAsync(keyName, trimOptions, mountPoint).Wait();

            var retrieveTrimmedKey = _authenticatedVaultClient.V1.Secrets.Transit.ReadEncryptionKeyAsync(keyName, mountPoint).Result;
            DisplayJson(retrieveTrimmedKey);
            Assert.True(retrieveTrimmedKey.Data.MinimumAvailableVersion == 2);

            // Generate Data Key
            var dataKeyOptions = new DataKeyRequestOptions
            {
                DataKeyType = TransitDataKeyType.wrapped,
                Base64EncodedContext = encodedContext,
                // Nonce = nonce
            };

            Secret<DataKeyResponse> generateDataKeyResponse = _authenticatedVaultClient.V1.Secrets.Transit.GenerateDataKeyAsync(keyName, dataKeyOptions, mountPoint).Result;
            DisplayJson(generateDataKeyResponse);
            Assert.Null(generateDataKeyResponse.Data.Base64EncodedPlainText);
            Assert.NotNull(generateDataKeyResponse.Data.CipherText);

            // delete a key

            allKeys = _authenticatedVaultClient.V1.Secrets.Transit.ReadAllEncryptionKeysAsync(mountPoint).Result;
            DisplayJson(allKeys);

            _authenticatedVaultClient.V1.Secrets.Transit.UpdateEncryptionKeyConfigAsync(secondConvergentKeyName,
                new UpdateKeyRequestOptions
                {
                    DeletionAllowed = true
                }, mountPoint).Wait();
            
            _authenticatedVaultClient.V1.Secrets.Transit.DeleteEncryptionKeyAsync(secondConvergentKeyName, mountPoint).Wait();

            var newAllKeys = _authenticatedVaultClient.V1.Secrets.Transit.ReadAllEncryptionKeysAsync(mountPoint).Result;
            DisplayJson(newAllKeys);
            Assert.Equal(allKeys.Data.Keys.Count() - 1, newAllKeys.Data.Keys.Count());

            // Run Cache
            var cacheResult = _authenticatedVaultClient.V1.Secrets.Transit.ReadCacheConfigAsync(mountPoint).Result;
            DisplayJson(cacheResult);
            Assert.True(0 == cacheResult.Data.Size || 10 <= cacheResult.Data.Size);

            var newSize = cacheResult.Data.Size == 0 ? 25 : cacheResult.Data.Size + 1;
            var cacheOptions = new CacheConfigRequestOptions { Size = newSize };
            _authenticatedVaultClient.V1.Secrets.Transit.SetCacheConfigAsync(cacheOptions, mountPoint).Wait();
            cacheResult = _authenticatedVaultClient.V1.Secrets.Transit.ReadCacheConfigAsync(mountPoint).Result;
            DisplayJson(cacheResult);
            Assert.Equal(cacheOptions.Size, cacheResult.Data.Size);

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(mountPoint).Wait();
        }
    }
}