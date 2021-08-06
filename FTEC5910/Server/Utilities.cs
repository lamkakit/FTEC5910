using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FTEC5910.Server
{
    public class Utilities
    {
        private static readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        public static T GenerateNonce<T>(int length)
        {
            // a default length could be specified instead of being parameterized
            var data = new byte[length];
            random.GetNonZeroBytes(data);
            if (typeof(T) == typeof(byte[]))
                return (data as dynamic);
            else
                return Convert.ToBase64String(data) as dynamic;
        }
    }
}
