using System.Security.Cryptography;
using System.Text;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý mã hóa.
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// Mã hóa chuỗi với phương thức MD5.
        /// </summary>
        /// <param name="input">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi mã hóa MD5.</returns>
        public static string MD5Hash(string input)
        {
            var hashBytes = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));

            var stringBuilder = new StringBuilder();
            foreach (var hashByte in hashBytes)
                stringBuilder.Append(hashByte.ToString("X2"));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Mã hóa chuỗi với phương thức SHA1.
        /// </summary>
        /// <param name="input">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi mã hóa SHA1.</returns>
        public static string SHA1Hash(string input)
        {
            var hashBytes = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(input));

            var stringBuilder = new StringBuilder();
            foreach (var hashByte in hashBytes)
                stringBuilder.Append(hashByte.ToString("X2"));

            return stringBuilder.ToString();
        }
    }
}