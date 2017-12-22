using VaultSharp.Core;

namespace VaultSharp.Backends.System.MFA.Okta
{
    internal class OktaProvider : MFAProviderBase<OktaConfig>, IOkta
    {
        public OktaProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "okta";
    }
}