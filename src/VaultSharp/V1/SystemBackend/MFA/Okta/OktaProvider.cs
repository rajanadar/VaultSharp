using VaultSharp.V1.Core;

namespace VaultSharp.V1.SystemBackend.MFA.Okta
{
    internal class OktaProvider : MFAProviderBase<OktaConfig>, IOkta
    {
        public OktaProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "okta";
    }
}