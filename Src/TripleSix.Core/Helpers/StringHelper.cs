using System.Linq;
using System.Text.RegularExpressions;

namespace TripleSix.Core.Helpers
{
    public static class StringHelper
    {
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ",
        };

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static bool IsNotNullOrWhiteSpace(this string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        public static string RemoveVietnameseSign(this string text)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    text = text.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return text;
        }

        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var texts = SplitCase(text).Select(x => char.ToUpper(x[0]) + x.Substring(1));
            var result = string.Join(string.Empty, texts);
            result = char.ToLower(result[0]) + result.Substring(1);

            return result;
        }

        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var texts = SplitCase(text);
            return string.Join("_", texts).ToLower();
        }

        public static string ToKebabCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var texts = SplitCase(text);
            return string.Join("-", texts).ToLower();
        }

        private static string[] SplitCase(string text)
        {
            return Regex.Replace(text, @"([a-z0-9])([A-Z])", "$1 $2")
                .Split(' ', '-', '_')
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToArray();
        }
    }
}