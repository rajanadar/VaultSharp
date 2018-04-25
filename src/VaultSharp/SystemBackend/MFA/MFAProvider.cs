using VaultSharp.Core;
using VaultSharp.SystemBackend.MFA.Duo;
using VaultSharp.SystemBackend.MFA.Okta;
using VaultSharp.SystemBackend.MFA.PingID;
using VaultSharp.SystemBackend.MFA.TOTP;

namespace VaultSharp.SystemBackend.MFA
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