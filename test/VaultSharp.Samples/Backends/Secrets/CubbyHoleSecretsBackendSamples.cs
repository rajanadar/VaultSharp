using System.Collections.Generic;
using System.Linq;
using VaultSharp.Core;
using Xunit;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunCubbyHoleSecretsBackendSamples()
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

    }
}