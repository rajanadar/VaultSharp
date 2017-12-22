using VaultSharp.Core;

namespace VaultSharp.Backends.System.MFA.TOTP
{
    internal class TOTPProvider : MFAProviderBase<TOTPConfig>, ITOTP
    {
        public TOTPProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "totp";
    }
}