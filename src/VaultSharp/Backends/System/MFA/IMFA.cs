using VaultSharp.Backends.System.MFA.Duo;
using VaultSharp.Backends.System.MFA.Okta;
using VaultSharp.Backends.System.MFA.PingID;
using VaultSharp.Backends.System.MFA.TOTP;

namespace VaultSharp.Backends.System.MFA
{
    /// <summary>
    /// The MFA interface.
    /// </summary>
    public interface IMFA
    {
        IDuo Duo { get; }

        IOkta Okta { get; }

        IPingID PingID { get; }

        ITOTP TOTP { get; }
    }
}