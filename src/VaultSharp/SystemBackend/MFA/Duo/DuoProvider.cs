using VaultSharp.Core;

namespace VaultSharp.SystemBackend.MFA.Duo
{
    internal class DuoProvider : MFAProviderBase<DuoConfig>, IDuo
    {
        public DuoProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "duo";
    }
}