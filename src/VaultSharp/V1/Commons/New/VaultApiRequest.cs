
using System.Threading;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents a Vault Request with no input data.
    /// </summary>
    public class VaultApiRequest
    {
        public string WrapTimeToLive { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}