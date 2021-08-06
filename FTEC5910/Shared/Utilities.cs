using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


namespace FTEC5910.Shared
{
    public class Utilities
    {
        //private static readonly Random random = new Random();

        public static T GenerateNonce<T>(int length)
        {
            // a default length could be specified instead of being parameterized
            //var data = new byte[length];
            //random.NextBytes(data);
            //if (typeof(T) == typeof(byte[]))
            //    return (data as dynamic);
            //else
            //    return Convert.ToBase64String(data) as dynamic;

            RandomNumberGenerator random = RandomNumberGenerator.Create();
            var data = new byte[length];
            random.GetNonZeroBytes(data);
            if (typeof(T) == typeof(byte[]))
                return (data as dynamic);
            else
                return Convert.ToBase64String(data) as dynamic;

        }

        public static string GetDescriptionFromEnum(Enum value)
        {
            if (value == null) return "";
            DescriptionAttribute attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        //https://kashifsoofi.github.io/cryptography/aes-in-csharp-using-bouncycastle/
        //https://github.com/kashifsoofi/crypto-sandbox/blob/master/dotnet/src/Sandbox.Crypto/AesBcCrypto.cs
        public static string Encrypt(string text) 
        {
            string key = "ftec5920";
            byte[] iv = new byte[16];
            var keyParameters = CreateKeyParameters(Encoding.UTF8.GetBytes(key), iv);
            var cipher = CipherUtilities.GetCipher("AES");
            cipher.Init(true, keyParameters);

            var plainTextData = Encoding.UTF8.GetBytes(text);
            var cipherText = cipher.DoFinal(plainTextData);

            return Convert.ToBase64String(cipherText);
        }
        private static ICipherParameters CreateKeyParameters(byte[] key, byte[] iv)
        {
            var keyParameter = new KeyParameter(key);
            return new ParametersWithIV(keyParameter, iv);
   
        }
    }
}
