using System.Security.Cryptography;
using System.Text;

namespace TripleSix.Core.Helpers
{
    public static class HashHelper
    {
        public static string MD5Hash(string input)
        {
            var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
                sb.Append(hashByte.ToString("X2"));

            return sb.ToString();
        }

        public static string SHA1Hash(string input)
        {
            var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(input));

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
                sb.Append(hashByte.ToString("X2"));

            return sb.ToString();
        }
    }
}