using System.Linq;

using Org.BouncyCastle.Security;

namespace Kriptal.Crypto
{
    public class RandomGeneration
    {
        private SecureRandom _cryptoRandom;

        public RandomGeneration()
        {
            _cryptoRandom = new SecureRandom();
        }

        public char LowercaseLetter => (char)_cryptoRandom.Next(97, 122);
        public char UppercaseLetter => (char)_cryptoRandom.Next(65, 90);
        public char Number => (char)_cryptoRandom.Next(48, 57);
        public char Symbol => (char)_cryptoRandom.Next(33, 47);

        public string GetRandomPassword()
        {
            var randomPassword = new char[8];

            randomPassword[0] = LowercaseLetter;
            randomPassword[1] = UppercaseLetter;
            randomPassword[2] = Number;
            randomPassword[3] = Symbol;

            for (int i = 4; i <= 7; i++)
            {
                var randomSelection = _cryptoRandom.Next(0, 3);
                switch (randomSelection)
                {
                    case 0:
                        randomPassword[i] = LowercaseLetter;
                        break;
                    case 1:
                        randomPassword[i] = UppercaseLetter;
                        break;
                    case 2:
                        randomPassword[i] = Number;
                        break;
                    case 3:
                        randomPassword[i] = Symbol;
                        break;
                    default:
                        randomPassword[i] = Symbol;
                        break;
                }
            }

            randomPassword = randomPassword.OrderBy(x => _cryptoRandom.Next()).ToArray();

            return new string(randomPassword);
        }
    }
}
