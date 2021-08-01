using System;
using System.Security.Cryptography;

namespace FTEC5910.Shared
{
    public class Utilities
    {
        private static readonly Random random = new Random();

        public static T GenerateNonce<T>(int length)
        {
            // a default length could be specified instead of being parameterized
            var data = new byte[length];
            random.NextBytes(data);
            if (typeof(T) == typeof(byte[]))
                return (data as dynamic);
            else
                return Convert.ToBase64String(data) as dynamic;
        }
    }
}
