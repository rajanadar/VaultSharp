using System;
using System.IO;
using System.Text;
using System.Threading;
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
        private static readonly ReaderWriterLockSlim FileLock = new ReaderWriterLockSlim();

        public CloudFoundrySignature GetSignatureToken(string roleName)
        {
            var instanceCert = GetBodyFromFile("CF_INSTANCE_CERT");

            var signingTime = Iso816UtcNow();
            var stringToSign = $"{signingTime}{instanceCert}{roleName}";

            var instanceKey = GetBodyFromFile("CF_INSTANCE_KEY");

            var data = Encoding.UTF8.GetBytes(stringToSign);
            var signatureKey = GenerateSignature(instanceKey, data);

            var token = new CloudFoundrySignature
            {
                InstanceCert = instanceCert,
                SigningTime = signingTime,
                RoleName = roleName,
                SignatureKey = signatureKey
            };

            return token;
        }

        private string GenerateSignature(string privateKeyPem, byte[] data)
        {
            byte[] keyBytes;
            using (var reader = new StringReader(privateKeyPem))
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

        private string GetBodyFromFile(string filePath)
        {
            var path = Environment.GetEnvironmentVariable(filePath);
            if (path == null) { return null; }

            try
            {
                FileLock.EnterReadLock();
                return File.ReadAllText(path);
            }
            finally
            {
                FileLock.ExitReadLock();
            }

        }
        
        private string Iso816UtcNow()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
