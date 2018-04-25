using VaultSharp.V1.Core;
using VaultSharp.V1.SystemBackend.MFA.Duo;
using VaultSharp.V1.SystemBackend.MFA.Okta;
using VaultSharp.V1.SystemBackend.MFA.PingID;
using VaultSharp.V1.SystemBackend.MFA.TOTP;

namespace VaultSharp.V1.SystemBackend.MFA
{
    /// <summary>
    /// MFA provider.
    /// </summary>
    internal class MFAProvider : IMFA
    {
        public MFAProvider(Polymath polymath)
        { 
            Duo = new DuoProvider(polymath);
            Okta = new OktaProvider(polymath);
            PingID = new PingIDProvider(polymath);
            TOTP = new TOTPProvider(polymath);
        }

        public IDuo Duo { get; }

        public IOkta Okta { get; }

        public IPingID PingID { get; }

        public ITOTP TOTP { get; }
    }
}