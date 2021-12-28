using System.Linq;
using System.Text.RegularExpressions;

namespace CpTech.Core.Extensions
{
    public static class StringExtension
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

        public static string RemoveVietnameseSign(this string text)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    text = text.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return text;
        }

        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            text = string.Join(" ", text.Split(" ").Where(x => x.Length > 0));
            text = text.Replace(" ", "_");
            return Regex.Replace(text, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string ToKebabCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            text = string.Join(" ", text.Split(" ").Where(x => x.Length > 0));
            text = text.Replace(" ", "-");
            return Regex.Replace(text, @"([a-z0-9])([A-Z])", "$1-$2").ToLower();
        }

        public static string ToCamelCase(this string text)
        {
            return string.IsNullOrWhiteSpace(text) || text.Length < 2
                ? text
                : char.ToLowerInvariant(text[0]) + text.Substring(1);
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static bool IsNotNullOrWhiteSpace(this string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}