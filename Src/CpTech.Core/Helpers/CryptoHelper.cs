using System.Security.Cryptography;
using System.Text;

namespace CpTech.Core.Helpers
{
    public static class CryptoHelper
    {
        public static string MD5Hash(string input, string key = null)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                input += key;
            }

            var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}