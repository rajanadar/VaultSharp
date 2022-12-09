using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Kubernetes
{
    /// <summary>
    /// Kubernetes Secrets Engine.
    /// </summary>
    public interface IKubernetesSecretsEngine
    {
        /// <summary>
        /// Generate a service account token.
        /// </summary>
        /// <param name="kubernetesRoleName"><para>[required]</para>
        /// Name of the role to generate credentials for.
        /// </param>
        /// <param name="kubernetesNamespace"><para>[required]</para>
        /// The name of the Kubernetes namespace in which to generate the credentials.
        /// </param>
        /// <param name="clusterRoleBinding"><para>[optional]</para>
        /// If true, generate a ClusterRoleBinding to grant permissions across the whole cluster instead of within a namespace. 
        /// Requires the Vault role to have kubernetes_role_type set to ClusterRole.
        /// </param>
        /// <param name="timeToLive"><para>[optional]</para>
        /// The TTL of the generated Kubernetes service account token, specified in seconds or as a Go duration format string, e.g. "1h". 
        /// The TTL returned may be different from the TTL specified due to limits specified in Kubernetes, 
        /// Vault system-wide controls, or role-specific controls.
        /// </param>
        /// <param name="kubernetesBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Kubernetes backend. Defaults to <see cref="SecretsEngineMountPoints.Kubernetes" />
        /// Provide a value only if you have customized the Kubernetes mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="KubernetesCredentials" /> as the data.
        /// </returns>
        Task<Secret<KubernetesCredentials>> GetCredentialsAsync(string kubernetesRoleName, string kubernetesNamespace, bool clusterRoleBinding = false, string timeToLive = "", string kubernetesBackendMountPoint = null, string wrapTimeToLive = null);
    }
}