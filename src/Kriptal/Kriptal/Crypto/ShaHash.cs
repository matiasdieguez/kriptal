using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Kriptal.Crypto
{
    public class ShaHash
    {
        private SecureRandom _cryptoRandom;

        public ShaHash()
        {
            _cryptoRandom = new SecureRandom();
        }

        public byte[] CreateSalt(int size)
        {
            byte[] salt = new byte[size];
            _cryptoRandom.NextBytes(salt);
            return salt;
        }

        public string Pbkdf2Sha256GetHash(string password, string saltAsBase64String, int iterations, int hashByteSize)
        {
            var saltBytes = Convert.FromBase64String(saltAsBase64String);

            var hash = Pbkdf2Sha256GetHash(password, saltBytes, iterations, hashByteSize);

            return Convert.ToBase64String(hash);
        }

        public byte[] Pbkdf2Sha256GetHash(string password, byte[] salt, int iterations, int hashByteSize)
        {
            var pdb = new Pkcs5S2ParametersGenerator(new Org.BouncyCastle.Crypto.Digests.Sha256Digest());
            pdb.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt,
                         iterations);
            var key = (KeyParameter)pdb.GenerateDerivedMacParameters(hashByteSize * 8);
            return key.GetKey();
        }

        public bool ValidatePassword(string password, string salt, int iterations, int hashByteSize, string hashAsBase64String)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] actualHashBytes = Convert.FromBase64String(hashAsBase64String);
            return ValidatePassword(password, saltBytes, iterations, hashByteSize, actualHashBytes);
        }

        public bool ValidatePassword(string password, byte[] saltBytes, int iterations, int hashByteSize, byte[] actualGainedHasAsByteArray)
        {
            byte[] testHash = Pbkdf2Sha256GetHash(password, saltBytes, iterations, hashByteSize);
            return SlowEquals(actualGainedHasAsByteArray, testHash);
        }

        private bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        public string DeriveSha256Key(string password)
        {
            int iterations = 1; // The number of times to encrypt the password 
            int saltByteSize = 8; // the salt size 
            int hashByteSize = 256; // the final hash 

            var hash = new ShaHash();

            byte[] saltBytes = hash.CreateSalt(saltByteSize);
            string saltString = Convert.ToBase64String(saltBytes);
            string key = hash.Pbkdf2Sha256GetHash(password, saltString, iterations, hashByteSize);

            //var isValid = hash.ValidatePassword(password, saltBytes, iterations, hashByteSize, Convert.FromBase64String(key));
            return key;
        }

        public ShaResult DeriveShaKey(string password, int keySize)
        {
            int iterations = 1; // The number of times to encrypt the password 
            int saltByteSize = 8; // the salt size 
            int hashByteSize = keySize; // the final hash 

            var hash = new ShaHash();

            byte[] saltBytes = hash.CreateSalt(saltByteSize);
            string saltString = Convert.ToBase64String(saltBytes);
            string key = hash.Pbkdf2Sha256GetHash(password, saltString, iterations, hashByteSize);

            //var isValid = hash.ValidatePassword(password, saltBytes, iterations, hashByteSize, Convert.FromBase64String(key));
            return new ShaResult { Digest = key, Salt = saltBytes };
        }

        public ShaResult DeriveShaKey(string password, int keySize, byte[] saltBytes)
        {
            int iterations = 1; // The number of times to encrypt the password 
            int hashByteSize = keySize; // the final hash 

            var hash = new ShaHash();

            string saltString = Convert.ToBase64String(saltBytes);
            string key = hash.Pbkdf2Sha256GetHash(password, saltString, iterations, hashByteSize);

            //var isValid = hash.ValidatePassword(password, saltBytes, iterations, hashByteSize, Convert.FromBase64String(key));
            return new ShaResult { Digest = key, Salt = saltBytes };
        }
    }
}