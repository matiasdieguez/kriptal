using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Text;

namespace Kriptal.Crypto
{

    public class AesCrypto
    {
        public AesResult Encrypt(string input, string keyString)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var iv = new byte[16];
            new SecureRandom().NextBytes(iv);

            //Set up
            var engine = new AesEngine();
            var blockCipher = new CbcBlockCipher(engine); //CBC
            var cipher = new PaddedBufferedBlockCipher(blockCipher); //Default scheme is PKCS5/PKCS7
            var keyParam = new KeyParameter(Convert.FromBase64String(keyString));
            var keyParamWithIV = new ParametersWithIV(keyParam, iv, 0, 16);

            // Encrypt
            cipher.Init(true, keyParamWithIV);
            var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            var length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
            cipher.DoFinal(outputBytes, length); //Do the final block
            var encryptedInput = Convert.ToBase64String(outputBytes);
            return new AesResult { EncryptedText = encryptedInput, Iv = iv };
        }

        public string Decrypt(string input, string keyString, byte[] iv)
        {
            var inputBytes = Convert.FromBase64String(input);

            var engine = new AesEngine();
            var blockCipher = new CbcBlockCipher(engine); //CBC
            var cipher = new PaddedBufferedBlockCipher(blockCipher); //Default scheme is PKCS5/PKCS7
            var keyParam = new KeyParameter(Convert.FromBase64String(keyString));
            var keyParamWithIV = new ParametersWithIV(keyParam, iv, 0, 16);

            cipher.Init(false, keyParamWithIV);

            var comparisonBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            var length = cipher.ProcessBytes(inputBytes, comparisonBytes, 0);
            cipher.DoFinal(comparisonBytes, length); //Do the final block

            var decrypted = Encoding.UTF8.GetString(comparisonBytes, 0, comparisonBytes.Length);
            return decrypted;
        }
    }
}
