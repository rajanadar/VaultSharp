using System.Linq;
using System.Text;
using System;
using Xunit;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.Transit;
using VaultSharp.V1.Commons;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunTransitSecretsBackendSamples()
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