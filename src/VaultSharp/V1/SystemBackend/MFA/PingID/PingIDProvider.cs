using VaultSharp.V1.Core;

namespace VaultSharp.V1.SystemBackend.MFA.PingID
{
    internal class PingIDProvider : MFAProviderBase<PingIDConfig>, IPingID
    {
        public PingIDProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "pingid";
    }
}