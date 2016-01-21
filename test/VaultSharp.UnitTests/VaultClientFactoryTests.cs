using System;
using VaultSharp.Backends.Authentication.Models.Token;
using Xunit;

namespace VaultSharp.UnitTests
{
    /// <summary>
    /// VaultClientFactoryTests
    /// </summary>
    public class VaultClientFactoryTests
    {
        /// <summary>
        /// Constructor throws on invalid inputs.
        /// </summary>
        [Fact]
        public void ConstructorThrowsOnInvalidInputs()
        {
            Assert.Throws<ArgumentNullException>(
                () => VaultClientFactory.CreateVaultClient(null, new TokenAuthenticationInfo("test")));
        }

        /// <summary>
        /// Determines whether this instance [can instantiate with minimal parameters].
        /// </summary>
        [Fact]
        public void CanInstantiateWithMinimalParameters()
        {
            var address = new Uri("http://127.0.0.1:8200");

            var client1 = VaultClientFactory.CreateVaultClient(address, null);
            Assert.NotNull(client1);

            var client2 = VaultClientFactory.CreateVaultClient(address, new TokenAuthenticationInfo("test"));
            Assert.NotNull(client2);
        }
    }
}