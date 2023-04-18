namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý ngẫu nhiên.
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// Phát sinh số ngẫu nhiên trong khoảng chỉ định.
        /// </summary>
        /// <param name="min">Số nhỏ nhất cho phép.</param>
        /// <param name="max">Số lớn nhất cho phép.</param>
        /// <returns>Số ngẫu nhiên.</returns>
        public static int RandomNumber(int min, int max)
        {
            if (min >= max)
                throw new ArgumentException("min must be less than max");

            return new Random().Next(min, max);
        }

        /// <summary>
        /// Phát sinh chuỗi ngẫu nhiên.
        /// </summary>
        /// <param name="length">Độ dài chuỗi.</param>
        /// <param name="chars">Danh sách ký tự sử dụng.</param>
        /// <returns>Chuỗi ngẫu nhiên.</returns>
        public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length <= 0)
                throw new ArgumentException("must be > 0", nameof(length));
            if (chars.Length == 0)
                throw new ArgumentException("cannot be empty", nameof(chars));

            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}