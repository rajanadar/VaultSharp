
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

            var readAppRoleId = _authenticatedVaultClient.V1.Auth.AppRole.GetRoleIdAsync(roleName, mountPoint).Result;
            DisplayJson(readAppRoleId);
            Assert.NotNull(readAppRoleId.Data.RoleId);

            var newRoleId = Guid.NewGuid().ToString();
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleIdAsync(roleName, new RoleIdInfo
            { RoleId = newRoleId }, mountPoint).Wait();

            var readNewAppRoleId = _authenticatedVaultClient.V1.Auth.AppRole.GetRoleIdAsync(roleName, mountPoint).Result;
            DisplayJson(readNewAppRoleId);
            Assert.Equal(newRoleId, readNewAppRoleId.Data.RoleId);

            var pullOptions = new PullSecretIdRequestOptions
            {
                Metadata = "{ \"tag1\": \"production\" }",
                TimeToLive = 600,
                NumberOfUses = 50
            };

            var secretIdInfo = _authenticatedVaultClient.V1.Auth.AppRole.PullNewSecretIdAsync(roleName, pullOptions, mountPoint).Result;
            DisplayJson(secretIdInfo);
            Assert.Equal(pullOptions.TimeToLive, secretIdInfo.Data.SecretIdTimeToLive);
            Assert.Equal(pullOptions.NumberOfUses, secretIdInfo.Data.SecretIdNumberOfUses);

            var secretId = secretIdInfo.Data.SecretId;
            var secretIdAccessor = secretIdInfo.Data.SecretIdAccessor;

            var secretIdAccessors = _authenticatedVaultClient.V1.Auth.AppRole.ReadAllSecretIdAccessorsAsync(roleName, mountPoint).Result;
            DisplayJson(secretIdAccessors);
            Assert.True(secretIdAccessors.Data.Keys.Count() == 1);
            Assert.Equal(secretIdAccessor, secretIdAccessors.Data.Keys.First());

            var readSecretIdInfo = _authenticatedVaultClient.V1.Auth.AppRole.ReadSecretIdInfoAsync(roleName, secretId, mountPoint).Result;
            DisplayJson(readSecretIdInfo);
            Assert.Equal(secretIdInfo.Data.SecretIdAccessor, readSecretIdInfo.Data.SecretIdAccessor);

            var readSecretIdInfoByAccessor = _authenticatedVaultClient.V1.Auth.AppRole.ReadSecretIdInfoByAccessorAsync(roleName, secretIdAccessor, mountPoint).Result;
            DisplayJson(readSecretIdInfoByAccessor);
            Assert.Equal(secretIdInfo.Data.SecretIdAccessor, readSecretIdInfoByAccessor.Data.SecretIdAccessor);

            var pushOptions = new PushSecretIdRequestOptions
            {
                Metadata = "{ \"tag1\": \"production\" }",
                TimeToLive = 700,
                NumberOfUses = 60,
                SecretId = Guid.NewGuid().ToString()
            };

            var pushSecretIdInfo = _authenticatedVaultClient.V1.Auth.AppRole.PushNewSecretIdAsync(roleName, pushOptions, mountPoint).Result;
            DisplayJson(pushSecretIdInfo);
            Assert.Equal(pushOptions.SecretId, pushSecretIdInfo.Data.SecretId);
            Assert.Equal(pushOptions.TimeToLive, pushSecretIdInfo.Data.SecretIdTimeToLive);
            Assert.Equal(pushOptions.NumberOfUses, pushSecretIdInfo.Data.SecretIdNumberOfUses);

            var pushSecretId = pushSecretIdInfo.Data.SecretId;
            var pushSecretIdAccessor = pushSecretIdInfo.Data.SecretIdAccessor;

            secretIdAccessors = _authenticatedVaultClient.V1.Auth.AppRole.ReadAllSecretIdAccessorsAsync(roleName, mountPoint).Result;
            DisplayJson(secretIdAccessors);
            Assert.True(secretIdAccessors.Data.Keys.Count() == 2);

            var tidy = _authenticatedVaultClient.V1.Auth.AppRole.TidyTokensAsync(mountPoint).Result;
            DisplayJson(tidy);
            Assert.NotNull(tidy.Warnings.First());

            _authenticatedVaultClient.V1.Auth.AppRole.DestroySecretIdAsync(roleName, secretId, mountPoint).Wait();

            var destroyedSecret1 = _authenticatedVaultClient.V1.Auth.AppRole.ReadSecretIdInfoAsync(roleName, secretId, mountPoint).Result;

            _authenticatedVaultClient.V1.Auth.AppRole.DestroySecretIdByAccessorAsync(roleName, pushSecretIdAccessor, mountPoint).Wait();

            var destroyedSecret2 = _authenticatedVaultClient.V1.Auth.AppRole.ReadSecretIdInfoAsync(roleName, pushSecretId, mountPoint).Result;

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleAsync(roleName, mountPoint).Wait();

            notFoundException = Assert.ThrowsAsync<VaultApiException>(
                () => _authenticatedVaultClient.V1.Auth.AppRole.ReadAllRolesAsync(mountPoint)).Result;

            Assert.Equal((int)HttpStatusCode.NotFound, notFoundException.StatusCode);

            // all done. get rid of the backend.
            _authenticatedVaultClient.V1.System.UnmountAuthBackendAsync(newApproleAuthBackend.Path).Wait();
        }
    }
}