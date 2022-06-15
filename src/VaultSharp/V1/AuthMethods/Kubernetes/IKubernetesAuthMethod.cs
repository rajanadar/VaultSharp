using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.Kubernetes.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Kubernetes;

public interface IKubernetesAuthMethod
{
    /// <summary>
    ///     Reads the configuration of an existing Kubernetes Auth method.
    /// </summary>
    /// <param name="mountPoint">Mount point of the Kubernetes Auth method</param>
    Task<Secret<KubernetesConfig>> ReadKubernetesConfig(string mountPoint = AuthMethodDefaultPaths.Kubernetes);

    /// <summary>
    ///     The Kubernetes auth method validates service account JWTs and verifies their existence with the Kubernetes
    ///     TokenReview API.
    ///     This endpoint configures the public key used to validate the JWT signature and the necessary information to access
    ///     the Kubernetes API.
    /// </summary>
    /// <param name="config">Configuration, values which are not set will be ignored</param>
    /// <param name="mountPoint">Mount point of the Kubernetes Auth method</param>
    Task WriteKubernetesConfig(KubernetesConfig config, string mountPoint = AuthMethodDefaultPaths.Kubernetes);

    /// <summary>
    ///     Returns the previously registered role configuration.
    /// </summary>
    /// <param name="roleName">Name of the Kubernetes Role</param>
    /// <param name="mountPoint">Mount point of the Kubernetes Auth method</param>
    Task<Secret<KubernetesRole>> ReadKubernetesRole(string roleName,
        string mountPoint = AuthMethodDefaultPaths.Kubernetes);

    /// <summary>
    ///     Registers a role in the auth method. Role types have specific entities that can perform login operations against
    ///     this endpoint.
    ///     Constraints specific to the role type must be set on the role. These are applied to the authenticated entities
    ///     attempting to login.
    /// </summary>
    /// <param name="role">Name of the Kubernetes Role</param>
    /// <param name="mountPoint">Mount point of the Kubernetes Auth method</param>
    Task CreateKubernetesRole(KubernetesRole role, string mountPoint = AuthMethodDefaultPaths.Kubernetes);

    /// <summary>
    ///     Lists all the roles that are registered with the auth method.
    /// </summary>
    /// <param name="mountPoint">Mount point of the Kubernetes Auth method</param>
    Task<Secret<ListInfo>> ListKubernetesRoles(string mountPoint = AuthMethodDefaultPaths.Kubernetes);

    /// <summary>
    ///     Deletes the previously registered role.
    /// </summary>
    /// <param name="roleName">Name of the Kubernetes Role</param>
    /// <param name="mountPoint">Mount point of the Kubernetes Auth method</param>
    Task DeleteKubernetesRole(string roleName, string mountPoint = AuthMethodDefaultPaths.Kubernetes);
}