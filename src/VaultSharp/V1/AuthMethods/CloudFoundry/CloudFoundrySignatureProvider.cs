using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace VaultSharp.V1.AuthMethods.CloudFoundry.Signature
{
    public class CloudFoundrySignatureProvider
    {
        public static string GetSignature(DateTime dateTime, string cfInstanceCertContent, string roleName, string cfInstanceKeyContent)
        {
            var formattedSigningTime = GetFormattedSigningTime(dateTime);
            var stringToSign = $"{formattedSigningTime}{cfInstanceCertContent}{roleName}";

            var data = Encoding.UTF8.GetBytes(stringToSign);

            byte[] keyBytes;

            using (var reader = new StringReader(cfInstanceKeyContent))
            {
                var pemReader = new PemReader(reader);
                var pemObject = pemReader.ReadPemObject();
                keyBytes = pemObject.Content;
            }

            var seq = (Asn1Sequence)Asn1Object.FromByteArray(keyBytes);
            var rsa = RsaPrivateKeyStructure.GetInstance(seq);

            var signer = new PssSigner(new RsaEngine(), new Sha256Digest(), 222);
            signer.Init(true, new RsaKeyParameters(true, rsa.Modulus, rsa.PrivateExponent));
            signer.BlockUpdate(data, 0, data.Length);
            var signature = signer.GenerateSignature();

            return $"v1:{Convert.ToBase64String(signature)}";
        }

        public static string GetFormattedSigningTime(DateTime signingTime)
        {
            return signingTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
