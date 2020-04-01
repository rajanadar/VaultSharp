using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace VaultSharp.V1.AuthMethods.CloudFoundry.Token
{
    public class SignatureUtility 
    {
        private static readonly ReaderWriterLockSlim FileLock = new ReaderWriterLockSlim();

        public async Task<string> GetSignatureToken(string role)
        {
            var cfInstanceCert = await GetBodyFromFile("CF_INSTANCE_CERT");
            
            var signingTime = Iso816UtcNow();
            var stringToSign = $"{signingTime}{cfInstanceCert}{role}";
            
            var cfInstanceKey = await GetBodyFromFile("CF_INSTANCE_KEY");

            var data = Encoding.UTF8.GetBytes(stringToSign);
            var signatureKey = GenerateSignature(cfInstanceKey, data);

            var token = new Signature
            {
                CFInstanceCert = cfInstanceCert,
                SigningTime = signingTime,
                Role = role,
                SignatureKey = signatureKey
            };
            
            return JsonConvert.SerializeObject(token);
        }

        public string GenerateSignature(string privateKeyPem, byte[] data)
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

        private string Iso816UtcNow()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private async Task<string> GetBodyFromFile(string filePath)
        {
            var path = Environment.GetEnvironmentVariable(filePath);
            if (path == null)
            {
                return null;
            }

            try
            {
                FileLock.EnterReadLock();

                byte[] result;

                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    result = new byte[stream.Length];
                    await stream.ReadAsync(result, 0, (int)stream.Length);
                }

                return Encoding.UTF8.GetString(result);
            }
            finally
            {
                FileLock.ExitReadLock();
            }

          
        }

    }
}
