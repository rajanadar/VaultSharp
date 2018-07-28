using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.MFA.TOTP
{
    internal class TOTPProvider : MFAProviderBase<TOTPConfig>, ITOTP
    {
        public TOTPProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "totp";
    }
}