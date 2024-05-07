using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend;

namespace VaultSharp.V1
{
    internal class VaultClientV1 : IVaultClientV1
    {
        private readonly Polymath _polymath;

        public VaultClientV1(Polymath polymath)
        {
            _polymath = polymath;
            System = new SystemBackendProvider(polymath);
            Auth = new AuthMethodProvider(polymath);
            Secrets = new SecretsEngineProvider(polymath);
        }

        public ISecretsEngine Secrets { get; }

        public IAuthMethod Auth { get; }

        public ISystemBackend System { get; }
       
        public async Task<TResponse> MakeVaultApiRequest<TResponse>(string resourcePath, HttpMethod method) where TResponse : class
        {
            return await _polymath.MakeVaultApiRequest<TResponse>(resourcePath, method).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
    }
}
