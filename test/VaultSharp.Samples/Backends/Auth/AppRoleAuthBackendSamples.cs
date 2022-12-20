
using System;
using System.Linq;
using System.Net;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole.Models;
using Xunit;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunAppRoleAuthMethodSamples()
        {
            // approle is not mounted by default. so start there.

            var newApproleAuthBackend = new AuthMethod
            {
                Path = "approle_" + Guid.NewGuid().ToString(),
                Type = AuthMethodType.AppRole,
                Description = "AppRole auth - test cases",
            };

            _authenticatedVaultClient.V1.System.MountAuthBackendAsync(newApproleAuthBackend).Wait();

            // start real test cases.

            var mountPoint = newApproleAuthBackend.Path;

            var notFoundException = Assert.ThrowsAsync<VaultApiException>(
                () => _authenticatedVaultClient.V1.Auth.AppRole.ReadAllRolesAsync(mountPoint)).Result;

            Assert.Equal((int)HttpStatusCode.NotFound, notFoundException.StatusCode);

            var newAppRoleRole = new AppRoleRoleModel
            {
                BindSecretId = true,
                SecretIdBoundCIDRs = new System.Collections.Generic.List<string> { "192.168.129.23/17" },
                SecretIdNumberOfUses = 100,
                SecretIdTimeToLive = 24 * 60 * 60,
                LocalSecretIds = true,
                TokenTimeToLive = 23 * 60 * 60,
                TokenMaximumTimeToLive = 37 * 60 * 60,
                TokenPolicies = new System.Collections.Generic.List<string> { "dev", "test" },

                // raja todo. Does not seem to reflect.
                Policies = new System.Collections.Generic.List<string> { "devp", "testp", "qap" },

                TokenBoundCIDRs = new System.Collections.Generic.List<string> { "192.168.128.23/17" },
                TokenExplicitMaximumTimeToLive = 35 * 60 * 60,
                TokenNumberOfUses = 99,
                TokenPeriod = 19 * 60 * 60,
                TokenType = AuthTokenType.Service
            };

            var roleName = "approle_rolename_" + Guid.NewGuid();

            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleAsync(roleName, newAppRoleRole, mountPoint).Wait();

            var writtenRole = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleAsync(roleName, mountPoint).Result;
            DisplayJson(writtenRole);

            Assert.Equal(newAppRoleRole.TokenMaximumTimeToLive, writtenRole.Data.TokenMaximumTimeToLive);
            Assert.Equal(newAppRoleRole.TokenNumberOfUses, writtenRole.Data.TokenNumberOfUses);

            var roles = _authenticatedVaultClient.V1.Auth.AppRole.ReadAllRolesAsync(mountPoint).Result;
            DisplayJson(roles);
            Assert.True(roles.Data.Keys.Count() == 1);
            Assert.Equal(roleName, roles.Data.Keys.First());

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleAsync(roleName, mountPoint).Wait();

            notFoundException = Assert.ThrowsAsync<VaultApiException>(
                () => _authenticatedVaultClient.V1.Auth.AppRole.ReadAllRolesAsync(mountPoint)).Result;

            Assert.Equal((int)HttpStatusCode.NotFound, notFoundException.StatusCode);

            // all done. get rid of the backend.
            _authenticatedVaultClient.V1.System.UnmountAuthBackendAsync(newApproleAuthBackend.Path).Wait();
        }
    }
}