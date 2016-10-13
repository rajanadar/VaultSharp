using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests
{
    public class VaultClientTests
    {
        private static readonly Uri VaultUri = new Uri("http://dummy");

        private static readonly IAuthenticationInfo DummyAuthenticationInfo = new TokenAuthenticationInfo("test");

        [Fact]
        public void ConstructorThrowsOnInvalidInputs()
        {
            Assert.Throws<ArgumentNullException>(() => new VaultClient(null, DummyAuthenticationInfo));
        }

        [Fact]
        public void CanInstantiateWithMinimalParameters()
        {
            var client1 = new VaultClient(VaultUri, null);
            Assert.NotNull(client1);

            var client2 = new VaultClient(VaultUri, DummyAuthenticationInfo);
            Assert.NotNull(client2);

            var client3 = new VaultClient(VaultUri, DummyAuthenticationInfo, true);
            Assert.NotNull(client3);

            var client4 = new VaultClient(VaultUri, DummyAuthenticationInfo, true, TimeSpan.FromMinutes(3));
            Assert.NotNull(client4);
        }

        [Fact]
        public async Task NullTests()
        {
            var client = new VaultClient(VaultUri, DummyAuthenticationInfo, true, TimeSpan.FromMinutes(3));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.QuickRootTokenGenerationAsync(allMasterShareKeys: null, nonce: "any"));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.QuickUnsealAsync(allMasterShareKeys: null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.MountSecretBackendAsync(secretBackend: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.QuickMountSecretBackendAsync(secretBackendType: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.UnmountSecretBackendAsync(mountPoint: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.QuickUnmountSecretBackendAsync(secretBackendType: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetMountedSecretBackendConfigurationAsync(mountPoint: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.TuneSecretBackendConfigurationAsync(mountPoint: null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.EnableAuthenticationBackendAsync(authenticationBackend: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DisableAuthenticationBackendAsync(authenticationPath: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetMountedAuthenticationBackendConfigurationAsync(authenticationPath: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.TuneAuthenticationBackendConfigurationAsync(authenticationPath: null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetPolicyAsync(policyName: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WritePolicyAsync(policy: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WritePolicyAsync(new Policy {Name = null}));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeletePolicyAsync(policyName: null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.EnableAuditBackendAsync(auditBackend: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DisableAuditBackendAsync(mountPoint: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.HashWithAuditBackendAsync(mountPoint: null, inputToHash: "a"));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeSecretAsync(leaseId: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeAllSecretsOrTokensUnderPrefixAsync(pathPrefix: null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ForceRevokeAllSecretsOrTokensUnderPrefixAsync(pathPrefix: null));

            // add selectively

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.EnableMultiFactorAuthenticationAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteDuoAccessAsync(null, "a", "a", "a"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteDuoConfigurationAsync(null, "a", "a"));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ContinueRekeyAsync(null, "nonce"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ContinueRekeyAsync("masterShareKey", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ReadRawSecretAsync(null));

            await
                Assert.ThrowsAsync<ArgumentNullException>(
                    () => client.WriteRawSecretAsync(null, new Dictionary<string, object>()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteRawSecretAsync("a", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeleteRawSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ReadSecretAsync(null));
            await
                Assert.ThrowsAsync<ArgumentNullException>(
                    () => client.WriteSecretAsync(null, new Dictionary<string, object>()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeleteSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetTokenInfoAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeTokenAsync(null, true));
        }
    }
}