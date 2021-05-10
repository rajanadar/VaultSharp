using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    internal class TransformSecretsEngineProvider : ITransformSecretsEngine
    {
        private readonly Polymath _polymath;

        public TransformSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }
        
        public async Task<Secret<EncodedResponse>> EncodeAsync(string roleName, EncodeRequestOptions encodeRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(encodeRequestOptions, "encodeRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<EncodedResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transform, "/encode/" + roleName.Trim('/'), HttpMethod.Post, encodeRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DecodedResponse>> DecodeAsync(string roleName, DecodeRequestOptions decodeRequestOptions, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(decodeRequestOptions, "decodeRequestOptions");

            return await _polymath.MakeVaultApiRequest<Secret<DecodedResponse>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Transform, "/decode/" + roleName.Trim('/'), HttpMethod.Post, decodeRequestOptions, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}