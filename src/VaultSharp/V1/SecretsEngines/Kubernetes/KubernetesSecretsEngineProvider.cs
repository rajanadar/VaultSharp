using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Kubernetes
{
    internal class KubernetesSecretsEngineProvider : IKubernetesSecretsEngine
    {
        private readonly Polymath _polymath;

        public KubernetesSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<KubernetesCredentials>> GetCredentialsAsync(string kubernetesRoleName, string kubernetesNamespace, bool clusterRoleBinding = false, string timeToLive = "", string kubernetesBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(kubernetesRoleName, "kubernetesRoleName");
            Checker.NotNull(kubernetesNamespace, "kubernetesNamespace");

            var requestData = new
            {
                kubernetes_namespace = kubernetesNamespace,
                cluster_role_binding = clusterRoleBinding,
                ttl = timeToLive
            };

            return await _polymath.MakeVaultApiRequest<Secret<KubernetesCredentials>>(kubernetesBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Kubernetes, "/creds/" + kubernetesRoleName.Trim('/'), HttpMethod.Post, requestData: requestData,  wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}