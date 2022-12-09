using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Okta
{
    internal class OktaAuthMethodProvider : IOktaAuthMethod
    {
        private readonly Polymath _polymath;

        public OktaAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }

        public async Task<Secret<OktaVerifyPushChallengeResponse>> VerifyPushChallengeAsync(string nonce, string mountPoint = AuthMethodDefaultPaths.Okta)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(nonce, "nonce");

            return await _polymath.MakeVaultApiRequest<Secret<OktaVerifyPushChallengeResponse>>("v1/auth/" + mountPoint.Trim('/') + "/verify/nonce/" + nonce, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
