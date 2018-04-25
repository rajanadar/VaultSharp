using VaultSharp.V1.SystemBackend.MFA.Duo;
using VaultSharp.V1.SystemBackend.MFA.Okta;
using VaultSharp.V1.SystemBackend.MFA.PingID;
using VaultSharp.V1.SystemBackend.MFA.TOTP;

namespace VaultSharp.V1.SystemBackend.MFA
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