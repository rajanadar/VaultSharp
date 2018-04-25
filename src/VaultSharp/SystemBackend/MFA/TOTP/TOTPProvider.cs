using VaultSharp.Core;

namespace VaultSharp.SystemBackend.MFA.TOTP
{
    internal class TOTPProvider : MFAProviderBase<TOTPConfig>, ITOTP
    {
        public TOTPProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "totp";
    }
}