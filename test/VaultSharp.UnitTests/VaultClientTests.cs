using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.System.Models;
using Xunit;

namespace VaultSharp.UnitTests
{
    public class VaultClientTests
    {
        private static readonly IAuthenticationInfo DummyAuthenticationInfo = new TokenAuthenticationInfo("test");

        [Fact]
        public void ConstructorThrowsOnInvalidInputs()
        {
            Assert.Throws<ArgumentNullException>(() => new VaultClient(null, DummyAuthenticationInfo));
        }

        [Fact]
        public void CanInstantiateWithMinimalParameters()
        {
            var address = new Uri("http://127.0.0.1:8200");

            var client1 = new VaultClient(address, null);
            Assert.NotNull(client1);

            var client2 = new VaultClient(address, DummyAuthenticationInfo);
            Assert.NotNull(client2);

            var client3 = new VaultClient(address, DummyAuthenticationInfo, true);
            Assert.NotNull(client3);

            var client4 = new VaultClient(address, DummyAuthenticationInfo, true, TimeSpan.FromMinutes(3));
            Assert.NotNull(client4);
        }

        [Fact]
        public async Task NullTests()
        {
            var client = new VaultClient(new Uri("http://127.0.0.1:8200"), DummyAuthenticationInfo, true, TimeSpan.FromMinutes(3));

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.UnsealQuickAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetMountedSecretBackendConfigurationAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.MountSecretBackendAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.TuneSecretBackendConfigurationAsync(null, new SecretBackendConfiguration()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.TuneSecretBackendConfigurationAsync("test", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.UnmountSecretBackendAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RemountSecretBackendAsync(null, "a"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RemountSecretBackendAsync("a", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.EnableAuthenticationBackendAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DisableAuthenticationBackendAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.EnableMultiFactorAuthenticationAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteDuoAccessAsync(null, "a", "a", "a"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteDuoConfigurationAsync(null, "a", "a"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetPolicyAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WritePolicyAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WritePolicyAsync(new Policy()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeletePolicyAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.EnableAuditBackendAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DisableAuditBackendAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.HashWithAuditBackendAsync(null, "a"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RenewSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeAllSecretsUnderPrefixAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ContinueRekeyAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ReadRawSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteRawSecretAsync(null, new Dictionary<string, object>()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteRawSecretAsync("a", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeleteRawSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.ReadSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.WriteSecretAsync(null, new Dictionary<string, object>()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeleteSecretAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetTokenInfoAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeTokenAsync(null, true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RevokeAllTokensUnderPrefixAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.RenewTokenAsync(null));
        }
    }
}