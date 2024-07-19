using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý chuỗi.
    /// </summary>
    public static partial class StringHelper
    {
        /// <summary>
        /// Xóa dấu tiếng việt.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi sau khi bỏ các dấu tiếng việt.</returns>
        public static string ClearVietnameseCase(this string text)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    text = text.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return text;
        }

        /// <summary>
        /// Kiểm tra chuỗi có null, rỗng hoặc chỉ toàn chứa khoảng trống.
        /// </summary>
        /// <param name="text">Chuỗi cần kiểm tra.</param>
        /// <returns>Kết quả kiểm tra.</returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Kiểm tra chuỗi không null, không rỗng và không chỉ chứa khoảng trống.
        /// </summary>
        /// <param name="text">Chuỗi cần kiểm tra.</param>
        /// <returns>Kết quả kiểm tra.</returns>
        public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Cắt từ khỏi chuỗi.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý.</param>
        /// <returns>Danh sách các từ đã cắt.</returns>
        public static string[] SplitCase(this string text)
        {
            return Regex.Replace(text, @"([a-z0-9])([A-Z])", "$1 $2")
                .Split(' ', '-', '_')
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToArray();
        }

        /// <summary>
        /// Chuyển chuỗi sang dạng Camel Case.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi đã xử lý.</returns>
        public static string ToCamelCase(this string text)
        {
            var texts = SplitCase(text).Select(x => char.ToUpper(x[0]) + x[1..]);
            var result = string.Join(string.Empty, texts);
            result = char.ToLower(result[0]) + result[1..];

            return result;
        }

        /// <summary>
        /// Chuyển chuỗi sang dạng Snake Case.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi đã xử lý.</returns>
        public static string ToSnakeCase(this string text)
        {
            return string.Join("_", SplitCase(text)).ToLower();
        }

        /// <summary>
        /// Chuyển chuỗi sang dạng Kebab Case.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi đã xử lý.</returns>
        public static string ToKebabCase(this string text)
        {
            return string.Join("-", SplitCase(text)).ToLower();
        }

        /// <summary>
        /// Chuyển chuỗi sang dạng Title Case.
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý.</param>
        /// <returns>Chuỗi đã xử lý.</returns>
        public static string ToTitleCase(this string text)
        {
            var cases = text.Split(' ').Select(x => x.Length <= 1 ? x.ToUpper() : char.ToUpper(x[0]) + x[1..]);
            return string.Join(' ', cases);
        }
    }

    public static partial class StringHelper
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
    }
}