using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.UserPass.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.UserPass;

public interface IUserPassAuthMethod
{
    Task CreateOrUpdateUser(UserPassUser user, string mountPoint = AuthMethodDefaultPaths.UserPass);

    Task<Secret<ReadUserResponse>> ReadUser(string username, string mountPoint = AuthMethodDefaultPaths.UserPass);

    Task DeleteUser(string username, string mountPoint = null);

    Task UpdatePasswordOnUser(string username, string password, string mountPoint = AuthMethodDefaultPaths.UserPass);

    Task UpdatePoliciesOnUser(string username, List<string> policies,
        string mountPoint = AuthMethodDefaultPaths.UserPass);

    Task<Secret<ListInfo>> ListUsers(string mountPoint = AuthMethodDefaultPaths.UserPass, string wrapTimeToLive = null);
}