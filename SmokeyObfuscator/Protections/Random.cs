using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmokeyObfuscator.Protections
{
    internal class Random2
    {
        private static System.Random random = new System.Random();
        private static System.Random length = new System.Random();

        public static string GetRandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length.Next(5, 20)).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int GetRandomInt()
        {
            var ints = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new System.Random(ints).Next(0, 99999999);
        }
    }
}
