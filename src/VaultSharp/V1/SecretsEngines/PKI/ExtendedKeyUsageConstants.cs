using System.Collections.Generic;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class ExtendedKeyUsageConstants
    {
        public const string Any = "Any";
        public const string ServerAuth = "ServerAuth";
        public const string ClientAuth = "ClientAuth";
        public const string CodeSigning = "CodeSigning";
        public const string EmailProtection = "EmailProtection";
        public const string IPSECEndSystem = "IPSECEndSystem";
        public const string IPSECTunnel = "IPSECTunnel";
        public const string IPSECUser = "IPSECUser";
        public const string TimeStamping = "TimeStamping";
        public const string OCSPSigning = "OCSPSigning";
        public const string MicrosoftServerGatedCrypto = "MicrosoftServerGatedCrypto";
        public const string NetscapeServerGatedCrypto = "NetscapeServerGatedCrypto";
        public const string MicrosoftCommercialCodeSigning = "MicrosoftCommercialCodeSigning";
        public const string MicrosoftKernelCodeSigning = "MicrosoftKernelCodeSigning";

        public static IReadOnlyList<string> DefaultSSLKeyUsages = new List<string>()
        {
            ServerAuth,
            ClientAuth
        };
    }
}