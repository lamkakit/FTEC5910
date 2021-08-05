using System;
using System.ComponentModel;
using System.Linq;
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

        public static string GetDescriptionFromEnum(Enum value)
        {
            if (value == null) return "";
            DescriptionAttribute attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
