using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.MFA.PingID
{
    internal class PingIDProvider : AbstractMFAProviderBase<PingIDConfig>, IPingID
    {
        public PingIDProvider(Polymath polymath) : base(polymath)
        {
        }

        public override string Type => "pingid";
    }
}