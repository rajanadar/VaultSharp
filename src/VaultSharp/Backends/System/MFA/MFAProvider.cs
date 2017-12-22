using VaultSharp.Backends.System.MFA.Duo;
using VaultSharp.Backends.System.MFA.Okta;
using VaultSharp.Backends.System.MFA.PingID;
using VaultSharp.Backends.System.MFA.TOTP;
using VaultSharp.Core;

namespace VaultSharp.Backends.System.MFA
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