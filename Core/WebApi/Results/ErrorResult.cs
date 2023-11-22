using System.ComponentModel;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Error result.
    /// </summary>
    public class ErrorResult : BaseResult<BaseMeta>
    {
        /// <summary>
        /// Error result.
        /// </summary>
        /// <param name="httpStatusCode">Mã trạng thái http.</param>
        /// <param name="code">Mã lỗi.</param>
        /// <param name="message">Nội dung lỗi.</param>
        /// <param name="data">Dữ liệu lỗi.</param>
        /// <param name="stackTrace">Stack trace.</param>
        public ErrorResult(
            int httpStatusCode = 500,
            string code = "exception",
            string message = "internal server error",
            object? data = null,
            string? stackTrace = null)
            : base(false, httpStatusCode)
        {
            Error = new ErrorInfo(code, message, data);

            if (stackTrace.IsNullOrEmpty()) return;
            StackTrace = stackTrace.Replace("\r", string.Empty).Split('\n')
                .Select(x => x.Trim())
                .ToList();
        }

        /// <summary>
        /// Thông tin lỗi.
        /// </summary>
        [DisplayName("Thông tin lỗi")]
        public virtual ErrorInfo Error { get; set; }

        /// <summary>
        /// Stack trace.
        /// </summary>
        [DisplayName("Stack trace")]
        public virtual List<string>? StackTrace { get; set; }
    }
}
