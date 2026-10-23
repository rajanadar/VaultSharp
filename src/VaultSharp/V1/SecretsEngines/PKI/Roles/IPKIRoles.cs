using System.Threading;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.PKI.Issuers;

namespace VaultSharp.V1.SecretsEngines.PKI.Roles
{
    public interface IPKIRoles
    {
        Task<Secret<RolesData>> ListRoles(string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task<Secret<RoleData>> AddRole(RoleData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task<Secret<RoleData>> GetRole(string roleName, string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task DeleteRole(string roleName, string pkiBackendMountPoint = null, CancellationToken cancellationToken = default);
    }
}