using System.Collections.Generic;
using System;
using Xunit;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunKeyValueSecretsBackendSamples()
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

    }
}