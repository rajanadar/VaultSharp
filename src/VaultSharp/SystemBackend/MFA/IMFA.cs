using VaultSharp.SystemBackend.MFA.Duo;
using VaultSharp.SystemBackend.MFA.Okta;
using VaultSharp.SystemBackend.MFA.PingID;
using VaultSharp.SystemBackend.MFA.TOTP;

namespace VaultSharp.SystemBackend.MFA
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