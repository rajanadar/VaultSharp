using VaultSharp.Core;

namespace VaultSharp.Backends.System.MFA.PingID
{
    internal class PingIDProvider : MFAProviderBase<PingIDConfig>, IPingID
    {
        public PingIDProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "pingid";
    }
}