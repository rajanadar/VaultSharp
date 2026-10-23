using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.RabbitMQ
{
    internal class RabbitMQSecretsEngineProvider : IRabbitMQSecretsEngine
    {
        private readonly Polymath _polymath;

        public RabbitMQSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<UsernamePasswordCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.RabbitMQ, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureLeaseAsync(RabbitMQLease lease, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(lease, "lease");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.RabbitMQ, "/config/lease", HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateRoleAsync(string roleName, RabbitMQRole role, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(role, "role");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.RabbitMQ, "/roles/" + roleName.Trim('/'), HttpMethod.Post, role, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RabbitMQRole>> ReadRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<RabbitMQRole>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.RabbitMQ, "/roles/" + roleName.Trim('/'), HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.RabbitMQ, "/roles/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}