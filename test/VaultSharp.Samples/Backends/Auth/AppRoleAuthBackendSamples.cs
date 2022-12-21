
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
            Assert.Null(destroyedSecret1);

            _authenticatedVaultClient.V1.Auth.AppRole.DestroySecretIdByAccessorAsync(roleName, pushSecretIdAccessor, mountPoint).Wait();

            var destroyedSecret2 = _authenticatedVaultClient.V1.Auth.AppRole.ReadSecretIdInfoAsync(roleName, pushSecretId, mountPoint).Result;
            Assert.Null(destroyedSecret2);

            // policies crud

            var existingPolicies = _authenticatedVaultClient.V1.Auth.AppRole.ReadRolePoliciesAsync(roleName, mountPoint).Result;
            DisplayJson(existingPolicies);
            Assert.True(existingPolicies.Data.Policies.Count > 0);
            Assert.True(existingPolicies.Data.TokenPolicies.Count > 0);

            existingPolicies.Data.Policies.Add("qaaa");
            existingPolicies.Data.TokenPolicies.Add("raaa");
            var newPolicies = existingPolicies.Data;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRolePoliciesAsync(roleName, newPolicies, mountPoint).Wait();

            var readNewPolicies = _authenticatedVaultClient.V1.Auth.AppRole.ReadRolePoliciesAsync(roleName, mountPoint).Result;
            DisplayJson(readNewPolicies);
            Assert.True(readNewPolicies.Data.Policies.Count == 3);
            Assert.True(readNewPolicies.Data.TokenPolicies.Count == 3);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRolePoliciesAsync(roleName, mountPoint).Wait();
            readNewPolicies = _authenticatedVaultClient.V1.Auth.AppRole.ReadRolePoliciesAsync(roleName, mountPoint).Result;
            DisplayJson(readNewPolicies);
            Assert.Null(readNewPolicies.Data.Policies);
            Assert.False(readNewPolicies.Data.TokenPolicies.Any());

            // secret id num uses crud
            var existingSecretIdNumberOfUses = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdNumberOfUsesAsync(roleName, mountPoint).Result;
            DisplayJson(existingSecretIdNumberOfUses);
            Assert.True(existingSecretIdNumberOfUses.Data > 0);

            var newSecretIdNumberOfUses = DateTime.Now.Year + DateTime.Now.Millisecond;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleSecretIdNumberOfUsesAsync(roleName, newSecretIdNumberOfUses, mountPoint).Wait();

            var readNewSecretIdNumberOfUses = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdNumberOfUsesAsync(roleName, mountPoint).Result;
            DisplayJson(readNewSecretIdNumberOfUses);
            Assert.Equal(newSecretIdNumberOfUses, readNewSecretIdNumberOfUses.Data);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleSecretIdNumberOfUsesAsync(roleName, mountPoint).Wait();
            readNewSecretIdNumberOfUses = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdNumberOfUsesAsync(roleName, mountPoint).Result;
            DisplayJson(readNewSecretIdNumberOfUses);
            Assert.True(readNewSecretIdNumberOfUses.Data == 0);

            // secret id ttl crud

            var existingSecretIdTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(existingSecretIdTimeToLive);
            Assert.True(existingSecretIdTimeToLive.Data > 0);

            var newSecretIdTimeToLive = DateTime.Now.Year + DateTime.Now.Millisecond;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleSecretIdTimeToLiveAsync(roleName, newSecretIdTimeToLive, mountPoint).Wait();

            var readNewSecretIdTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(readNewSecretIdTimeToLive);
            Assert.Equal(newSecretIdTimeToLive, readNewSecretIdTimeToLive.Data);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleSecretIdTimeToLiveAsync(roleName, mountPoint).Wait();
            readNewSecretIdTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(readNewSecretIdTimeToLive);
            Assert.True(readNewSecretIdTimeToLive.Data == 0);

            // token ttl crud

            var existingTokenTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(existingTokenTimeToLive);
            Assert.True(existingTokenTimeToLive.Data > 0);

            var newTokenTimeToLive = DateTime.Now.Year + DateTime.Now.Millisecond;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleTokenTimeToLiveAsync(roleName, newTokenTimeToLive, mountPoint).Wait();

            var readNewTokenTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(readNewTokenTimeToLive);
            Assert.Equal(newTokenTimeToLive, readNewTokenTimeToLive.Data);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleTokenTimeToLiveAsync(roleName, mountPoint).Wait();
            readNewTokenTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(readNewTokenTimeToLive);
            Assert.True(readNewTokenTimeToLive.Data == 0);

            // token max ttl

            var existingTokenMaximumTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenMaximumTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(existingTokenMaximumTimeToLive);
            Assert.True(existingTokenMaximumTimeToLive.Data > 0);

            var newTokenMaximumTimeToLive = DateTime.Now.Year + DateTime.Now.Millisecond;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleTokenMaximumTimeToLiveAsync(roleName, newTokenMaximumTimeToLive, mountPoint).Wait();

            var readNewTokenMaximumTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenMaximumTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(readNewTokenMaximumTimeToLive);
            Assert.Equal(newTokenMaximumTimeToLive, readNewTokenMaximumTimeToLive.Data);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleTokenMaximumTimeToLiveAsync(roleName, mountPoint).Wait();
            readNewTokenMaximumTimeToLive = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenMaximumTimeToLiveAsync(roleName, mountPoint).Result;
            DisplayJson(readNewTokenMaximumTimeToLive);
            Assert.True(readNewTokenMaximumTimeToLive.Data == 0);

            // bind secret id

            var existingBindSecretId = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleBindSecretIdAsync(roleName, mountPoint).Result;
            DisplayJson(existingBindSecretId);
            Assert.True(existingBindSecretId.Data);

            var newBindSecretId = false;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleBindSecretIdAsync(roleName, newBindSecretId, mountPoint).Wait();

            var readNewBindSecretId = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleBindSecretIdAsync(roleName, mountPoint).Result;
            DisplayJson(readNewBindSecretId);
            Assert.Equal(newBindSecretId, readNewBindSecretId.Data);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleBindSecretIdAsync(roleName, mountPoint).Wait();
            readNewBindSecretId = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleBindSecretIdAsync(roleName, mountPoint).Result;
            DisplayJson(readNewBindSecretId);

            // raja todo
            Assert.True(readNewBindSecretId.Data); // ?? why true. Does it revert to original default on role?

            // secret id bound cidrs

            var existingSecretIdBoundCIDRs = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdBoundCIDRsAsync(roleName, mountPoint).Result;
            DisplayJson(existingSecretIdBoundCIDRs);
            Assert.True(existingSecretIdBoundCIDRs.Data.Count > 0);

            existingSecretIdBoundCIDRs.Data.Add("192.168.142.23/17");
            var newSecretIdBoundCIDRs = existingSecretIdBoundCIDRs.Data;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleSecretIdBoundCIDRsAsync(roleName, newSecretIdBoundCIDRs, mountPoint).Wait();

            var readNewSecretIdBoundCIDRs = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdBoundCIDRsAsync(roleName, mountPoint).Result;
            DisplayJson(readNewSecretIdBoundCIDRs);
            Assert.Equal(existingSecretIdBoundCIDRs.Data.Count, readNewSecretIdBoundCIDRs.Data.Count);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleSecretIdBoundCIDRsAsync(roleName, mountPoint).Wait();
            readNewSecretIdBoundCIDRs = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleSecretIdBoundCIDRsAsync(roleName, mountPoint).Result;
            DisplayJson(readNewSecretIdBoundCIDRs);
            Assert.Null(readNewSecretIdBoundCIDRs.Data);

            // token bound CIDRs

            var existingTokenBoundCIDRs = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenBoundCIDRsAsync(roleName, mountPoint).Result;
            DisplayJson(existingTokenBoundCIDRs);
            Assert.True(existingTokenBoundCIDRs.Data.Count > 0);

            existingTokenBoundCIDRs.Data.Add("192.168.167.23/18");
            var newTokenBoundCIDRs = existingTokenBoundCIDRs.Data;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRoleTokenBoundCIDRsAsync(roleName, newTokenBoundCIDRs, mountPoint).Wait();

            var readNewTokenBoundCIDRs = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenBoundCIDRsAsync(roleName, mountPoint).Result;
            DisplayJson(readNewTokenBoundCIDRs);
            Assert.Equal(existingTokenBoundCIDRs.Data.Count, readNewTokenBoundCIDRs.Data.Count);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleTokenBoundCIDRsAsync(roleName, mountPoint).Wait();
            readNewTokenBoundCIDRs = _authenticatedVaultClient.V1.Auth.AppRole.ReadRoleTokenBoundCIDRsAsync(roleName, mountPoint).Result;
            DisplayJson(readNewTokenBoundCIDRs);
            Assert.Null(readNewTokenBoundCIDRs.Data);

            // period cidr

            var existingPeriod = _authenticatedVaultClient.V1.Auth.AppRole.ReadRolePeriodAsync(roleName, mountPoint).Result;
            DisplayJson(existingPeriod);
            Assert.True(existingPeriod.Data > 0);

            var newPeriod = DateTime.Now.Year + DateTime.Now.Millisecond;
            _authenticatedVaultClient.V1.Auth.AppRole.WriteRolePeriodAsync(roleName, newPeriod, mountPoint).Wait();

            var readNewPeriod = _authenticatedVaultClient.V1.Auth.AppRole.ReadRolePeriodAsync(roleName, mountPoint).Result;
            DisplayJson(readNewPeriod);
            Assert.Equal(newPeriod, readNewPeriod.Data);

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRolePeriodAsync(roleName, mountPoint).Wait();
            readNewPeriod = _authenticatedVaultClient.V1.Auth.AppRole.ReadRolePeriodAsync(roleName, mountPoint).Result;
            DisplayJson(readNewPeriod);
            Assert.True(readNewPeriod.Data == 0);

            // all done.

            _authenticatedVaultClient.V1.Auth.AppRole.DeleteRoleAsync(roleName, mountPoint).Wait();

            notFoundException = Assert.ThrowsAsync<VaultApiException>(
                () => _authenticatedVaultClient.V1.Auth.AppRole.ReadAllRolesAsync(mountPoint)).Result;

            Assert.Equal((int)HttpStatusCode.NotFound, notFoundException.StatusCode);

            // all done. get rid of the backend.
            _authenticatedVaultClient.V1.System.UnmountAuthBackendAsync(newApproleAuthBackend.Path).Wait();
        }
    }
}