using System.Collections.Generic;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    public class KeyUsageConstants
    {
        public const string DigitalSignature = "DigitalSignature";
        public const string ContentCommitment = "ContentCommitment";
        public const string KeyEncipherment = "KeyEncipherment";
        public const string DataEncipherment = "DataEncipherment";
        public const string KeyAgreement = "KeyAgreement";
        public const string CertSign = "CertSign";
        public const string CRLSign = "CRLSign";
        public const string EncipherOnly = "EncipherOnly";
        public const string DecipherOnly = "DecipherOnly";

        public static IReadOnlyList<string> DefaultSSLKeyUsages = new List<string>()
        {
            DigitalSignature,
            KeyEncipherment,
            KeyAgreement
        };
    }
}