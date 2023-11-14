namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý exception.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Lấy tất cả message của exception.
        /// </summary>
        /// <param name="exception">Exception cần xử lý.</param>
        /// <returns>Danh sách message của exception và inner exception.</returns>
        public static string FullMessage(this Exception exception)
        {
            var result = new List<string>();

            var e = exception;
            while (e != null)
            {
                if (e.Message.IsNotNullOrEmpty()) result.Add(e.Message);
                e = e.InnerException;
            }

            return result.ToString(". ");
        }
    }
}
