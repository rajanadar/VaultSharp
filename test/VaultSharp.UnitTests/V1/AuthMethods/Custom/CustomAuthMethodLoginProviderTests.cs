using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.Custom;
using VaultSharp.V1.Commons;
using Xunit;

namespace VaultSharp.UnitTests.V1.AuthMethods.Custom
{
    public class CustomAuthMethodLoginProviderTests
    {
        [Fact]
        public async Task should_get_token_from_delegate()
        {
            const string testToken = "test-token";
            var customAuthInfo = new CustomAuthMethodInfo("test", () => Task.FromResult(testToken));

            CustomAuthMethodLoginProvider loginProvider = GetLoginProvider(customAuthInfo);

            var token = await loginProvider.GetVaultTokenAsync();
            
            Assert.Equal(testToken, token);
        }

        [Fact]
        public async Task should_set_returned_auth_info_from_delegate_if_auth_info_delegate_is_passed()
        {
            const string testToken = "test-token";
            
            var authInfo = new AuthInfo
            {
                ClientToken = testToken,
                LeaseDurationSeconds = 100,
                Renewable = true,
                Policies = new List<string> { "test-policy" }
            };
            
            var customAuthInfo = new CustomAuthMethodInfo("test", () => Task.FromResult(authInfo));

            CustomAuthMethodLoginProvider loginProvider = GetLoginProvider(customAuthInfo);

            await loginProvider.GetVaultTokenAsync();
            
            Assert.Equal(authInfo, customAuthInfo.ReturnedLoginAuthInfo);
        }
        
        [Fact]
        public async Task should_get_token_from_delegate_if_auth_info_delegate_is_passed()
        {
            const string testToken = "test-token";
            
            var authInfo = new AuthInfo
            {
                ClientToken = testToken,
                LeaseDurationSeconds = 100,
                Renewable = true,
                Policies = new List<string> { "test-policy" }
            };
            
            var customAuthInfo = new CustomAuthMethodInfo("test", () => Task.FromResult(authInfo));

            CustomAuthMethodLoginProvider loginProvider = GetLoginProvider(customAuthInfo);

            var token = await loginProvider.GetVaultTokenAsync();
            
            Assert.Equal(testToken, token);
        }
        
        [Fact]
        public async Task should_throw_exception_when_auth_info_is_null_if_auth_info_delegate_is_passed()
        {
            var customAuthInfo = new CustomAuthMethodInfo("test", () => Task.FromResult<AuthInfo>(null));

            CustomAuthMethodLoginProvider loginProvider = GetLoginProvider(customAuthInfo);

            var exception = await Assert.ThrowsAsync<Exception>(() => loginProvider.GetVaultTokenAsync());
            Assert.Equal("The call to the Custom Auth method did not yield a client token. Please verify your credentials.", exception.Message);
        }
        
        private static CustomAuthMethodLoginProvider GetLoginProvider(CustomAuthMethodInfo customAuthInfo)
        {
            var polymath = new Polymath(new VaultClientSettings("http://test-vault", customAuthInfo));

            var loginProvider = new CustomAuthMethodLoginProvider(customAuthInfo, polymath);
            return loginProvider;
        }
    }
}