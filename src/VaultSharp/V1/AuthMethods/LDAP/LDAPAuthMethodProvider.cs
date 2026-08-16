using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.LDAP
{
    internal class LDAPAuthMethodProvider : ILDAPAuthMethod
    {
        private readonly Polymath _polymath;

        public LDAPAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            _polymath = polymath;
        }

        public async Task WriteGroupAsync(string groupName, IList<string> policies, string mountPoint = AuthMethodDefaultPaths.LDAP)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(groupName, "groupName");

            var flatPolicies = string.Join(",", policies);

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/groups/" + groupName.Trim('/'), HttpMethod.Post, new PoliciesRequest { Policies = flatPolicies }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<List<string>>> ReadGroupAsync(string groupName, string mountPoint = AuthMethodDefaultPaths.LDAP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(groupName, "groupName");

            return await _polymath.MakeVaultApiRequest<Secret<List<string>>>("v1/auth/" + mountPoint.Trim('/') + "/groups/" + groupName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllGroupsAsync(string mountPoint = AuthMethodDefaultPaths.LDAP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/auth/" + mountPoint.Trim('/') + "/groups", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteGroupAsync(string groupName, string mountPoint = AuthMethodDefaultPaths.LDAP)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(groupName, "groupName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/groups/" + groupName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteUserAsync(string username, IList<string> policies, IList<string> groups, string mountPoint = "ldap")
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(username, "username");

            var flatPolicies = string.Join(",", policies ?? new List<string>());
            var flatGroups = string.Join(",", groups ?? new List<string>());

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/users/" + username.Trim('/'), HttpMethod.Post, new PoliciesGroupsRequest { Policies = flatPolicies, Groups = flatGroups }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> ReadUserAsync(string username, string mountPoint = "ldap", string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(username, "username");

            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/users/" + username.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllUsersAsync(string mountPoint = "ldap", string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/auth/" + mountPoint.Trim('/') + "/users", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteUserAsync(string username, string mountPoint = "ldap")
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(username, "username");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/users/" + username.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}