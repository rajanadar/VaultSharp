using System.Collections.Generic;
using System;
using Xunit;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;
using System.Linq;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunKeyValueSecretsBackendSamples()
        {
            Console.WriteLine("\n RunKeyValue 1 SecretsBackendSamples \n");
            RunKeyValue1SecretsBackendSamples();

            Console.WriteLine("\n RunKeyValue 2 SecretsBackendSamples \n");
            RunKeyValue2SecretsBackendSamples();
        }

        private static void RunKeyValue1SecretsBackendSamples()
        {
            var kv1MountPath = Guid.NewGuid().ToString();

            // mount a new v1 kv
            var kv1SecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.KeyValueV1,
                Config = new Dictionary<string, object>
                {
                    {  "version", "1" }
                },
                Path = kv1MountPath
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(kv1SecretsEngine).Wait();

            var kv1Path = Guid.NewGuid().ToString();

            var kv1DataValues = new Dictionary<string, object>
            {
                {"foo", "kv2"},
                {"foo2", 345 }
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(kv1Path, kv1DataValues, kv1MountPath).Wait();

            var kv1Paths1 = _authenticatedVaultClient.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("", kv1MountPath).Result;
            DisplayJson(kv1Paths1);
            Assert.True(kv1Paths1.Data.Keys.Count() == 1);

            var kv1Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync(kv1Path, kv1MountPath).Result;
            DisplayJson(kv1Secret);
            Assert.True(kv1Secret.Data.Count == 2);

            _authenticatedVaultClient.V1.Secrets.KeyValue.V1.DeleteSecretAsync(kv1Path, kv1MountPath).Wait();

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(kv1MountPath).Wait();
        }

        private static void RunKeyValue2SecretsBackendSamples()
        {
            var mountPoint = Guid.NewGuid().ToString();

            // mount a new v2 kv
            var kv2SecretsEngine = new SecretsEngine
            {
                Type = SecretsEngineType.KeyValueV2,
                Config = new Dictionary<string, object>
                {
                    {  "version", "2" }
                },
                Path = mountPoint
            };

            _authenticatedVaultClient.V1.System.MountSecretBackendAsync(kv2SecretsEngine).Wait();

            // wait for a second for vault to upgrade all keys
            System.Threading.Thread.Sleep(1000);

            var kv2Config = new KeyValue2ConfigModel
            {
                CASRequired = true,
                DeleteVersionAfter = "2h3m",
                MaxVersions = 6
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ConfigureAsync(kv2Config, mountPoint).Wait();

            var readConfig = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadConfigAsync(mountPoint).Result;
            DisplayJson(readConfig);
            Assert.Equal(kv2Config.MaxVersions, readConfig.Data.MaxVersions);

            var firstSecretPath = "common/first/" + Guid.NewGuid().ToString();
            var firstSecret = new Dictionary<string, object>
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };

            kv2Config.CASRequired = false;
            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ConfigureAsync(kv2Config, mountPoint).Wait();

            var currentMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(firstSecretPath, firstSecret, mountPoint: kv2SecretsEngine.Path).Result;
            DisplayJson(currentMetadata);
            Assert.True(currentMetadata.Data.Version == 1);

            var readKV2Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(firstSecretPath, mountPoint: kv2SecretsEngine.Path).Result;
            DisplayJson(readKV2Secret);
            Assert.True(readKV2Secret.Data.Data.Count == 2);

            var secondSecretPath = "common/second/" + Guid.NewGuid().ToString();
            var secondSecret = new Dictionary<string, object>
            {
                { "k1", false },
                { "k2", 100 },
                { "k3", "hmm" }
            };
            var currentMetadataForSecondSecret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(secondSecretPath, secondSecret, mountPoint: kv2SecretsEngine.Path).Result;
            DisplayJson(currentMetadataForSecondSecret);
            Assert.True(currentMetadataForSecondSecret.Data.Version == 1);

            var subkeys = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretSubkeysAsync(secondSecretPath, mountPoint: mountPoint).Result;
            DisplayJson(subkeys);
            Assert.True(subkeys.Data.Subkeys.Count == 3);

            var secretPathsUnderCommon = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync("common", mountPoint: mountPoint).Result;
            DisplayJson(secretPathsUnderCommon);
            Assert.True(secretPathsUnderCommon.Data.Keys.Count() == 2);

            var firstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint).Result;
            DisplayJson(firstSecretMetadata);
            Assert.True(firstSecretMetadata.Data.CurrentVersion == 1);

            firstSecret.Add("one-more", "42");

            var newFirstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(firstSecretPath, firstSecret, mountPoint: mountPoint).Result;
            DisplayJson(newFirstSecretMetadata);
            Assert.True(newFirstSecretMetadata.Data.Version == 2);

            firstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint).Result;
            DisplayJson(firstSecretMetadata);
            Assert.True(firstSecretMetadata.Data.CurrentVersion == 2);

            readKV2Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(firstSecretPath, version: 2, mountPoint: kv2SecretsEngine.Path).Result;
            DisplayJson(readKV2Secret);
            Assert.True(readKV2Secret.Data.Data.Count == 3);

            readKV2Secret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(firstSecretPath, version: 1, mountPoint: kv2SecretsEngine.Path).Result;
            DisplayJson(readKV2Secret);
            Assert.True(readKV2Secret.Data.Data.Count == 2);

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.DeleteSecretVersionsAsync(firstSecretPath, new List<int> { 2 }, mountPoint).Wait();
            firstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint).Result;
            DisplayJson(firstSecretMetadata);
            Assert.True(firstSecretMetadata.Data.CurrentVersion == 2);  // raja todo why?
 
            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.UndeleteSecretVersionsAsync(firstSecretPath, new List<int> { 2 }, mountPoint).Wait();
            firstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint).Result;
            DisplayJson(firstSecretMetadata);
            Assert.True(firstSecretMetadata.Data.CurrentVersion == 2);

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.DeleteSecretAsync(firstSecretPath, mountPoint).Wait();
            firstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint).Result;
            DisplayJson(firstSecretMetadata);
            Assert.True(firstSecretMetadata.Data.CurrentVersion == 2);   // raja todo why?

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.UndeleteSecretVersionsAsync(firstSecretPath, new List<int> { 2 }, mountPoint).Wait();
            firstSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint).Result;
            DisplayJson(firstSecretMetadata);
            Assert.True(firstSecretMetadata.Data.CurrentVersion == 2);

            // patch
            var patchSecretPath = Guid.NewGuid().ToString();
            var initialData = new Dictionary<string, object>
            {
                { "k1", 1},
                { "k2", 1.5 }
            };
            var initialPatchSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(patchSecretPath, initialData, mountPoint: mountPoint).Result;
            DisplayJson(initialPatchSecretMetadata);
            Assert.True(initialPatchSecretMetadata.Data.Version == 1);

            var patchSecretDataRequest = new PatchSecretDataRequest()
            {
                Data = new Dictionary<string, object>
                {
                    { "k2", 2 },
                    { "k3", 3 }
                }
            };

            var postPatchSecretMetadata = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.PatchSecretAsync(patchSecretPath,
                patchSecretDataRequest, mountPoint).Result;
            DisplayJson(postPatchSecretMetadata);
            Assert.True(postPatchSecretMetadata.Data.Version == 2);

            var readPostPatchSecret = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(patchSecretPath, mountPoint: mountPoint).Result;
            DisplayJson(readPostPatchSecret);
            Assert.True(readPostPatchSecret.Data.Data.Count == 3);
            Assert.True((long)readPostPatchSecret.Data.Data["k2"] == 2);            

            var writeCustomMetadataRequest = new CustomMetadataRequest
            {
                CustomMetadata = new Dictionary<string, string>
                {
                    { "owner", "system"},
                    { "expired_in", "20331010"}
                }
            };

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.WriteSecretMetadataAsync(firstSecretPath, writeCustomMetadataRequest, mountPoint: mountPoint).Wait();

            var kv2metadata2 = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint: mountPoint).Result;
            DisplayJson(kv2metadata2);

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

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.PatchSecretMetadataAsync(firstSecretPath, patchCustomMetadataRequest, mountPoint: mountPoint).Wait();

            var kv2metadata3 = _authenticatedVaultClient.V1.Secrets.KeyValue.V2.ReadSecretMetadataAsync(firstSecretPath, mountPoint: mountPoint).Result;
            DisplayJson(kv2metadata3);

            Assert.True(kv2metadata3.Data.CustomMetadata.Count == 3);
            Assert.True(kv2metadata3.Data.CustomMetadata["expired_in"] == "20341010");
            Assert.True(kv2metadata3.Data.CustomMetadata["owner"] == "system");
            Assert.True(kv2metadata3.Data.CustomMetadata["locale"] == "EN");

            _authenticatedVaultClient.V1.Secrets.KeyValue.V2.DestroySecretVersionsAsync(firstSecretPath, new List<int> { firstSecretMetadata.Data.CurrentVersion }, mountPoint: mountPoint).Wait();

            _authenticatedVaultClient.V1.System.UnmountSecretBackendAsync(kv2SecretsEngine.Path).Wait();
        }
    }
}