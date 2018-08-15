using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.MFA.Okta
{
    internal class OktaProvider : AbstractMFAProviderBase<OktaConfig>, IOkta
    {
        public OktaProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "okta";
    }
}