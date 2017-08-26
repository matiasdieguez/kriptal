using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kriptal.Crypto
{

    public class RsaCrypto
    {
        public async Task<KriptalRsaKeyPair> CreateKeyPair()
        {
            var kpgen = new RsaKeyPairGenerator();
            var privateKey = string.Empty;
            var publicKey = string.Empty;

            kpgen.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            await Task.Run(() => 
            {
                var keyPair = kpgen.GenerateKeyPair();

                PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                privateKey = Convert.ToBase64String(pkInfo.GetDerEncoded());

                SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
                publicKey = Convert.ToBase64String(info.GetDerEncoded());
            });

            return new KriptalRsaKeyPair { PrivateKey = privateKey, PublicKey = publicKey };
        }

        public string EncryptWithPublic(string clearText, string publicKey)
        {
            publicKey = "-----BEGIN PUBLIC KEY-----" + Environment.NewLine + publicKey + Environment.NewLine + "-----END PUBLIC KEY-----";
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;

        }

        public string EncryptWithPrivate(string clearText, string privateKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(privateKey))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyPair.Private);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }

        // Decryption:

        public string DecryptWithPrivate(string base64Input, string privateKey)
        {
            privateKey = "-----BEGIN PRIVATE KEY-----" + Environment.NewLine + privateKey + Environment.NewLine + "-----END PRIVATE KEY-----";

            var bytesToDecrypt = Convert.FromBase64String(base64Input);

            var decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(privateKey))
            {
                var r = new PemReader(txtreader).ReadObject();
                decryptEngine.Init(false, (ICipherParameters)r);
            }

            var bytes = decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length);
            var decrypted = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return decrypted;
        }

        public string DecryptWithPublic(string base64Input, string publicKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);

            var decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyParameter);
            }

            var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length), 0, bytesToDecrypt.Length);
            return decrypted;
        }
    }

}
