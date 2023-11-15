using System.ComponentModel;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class ErrorResult : BaseResult<BaseMeta>
    {
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

        [DisplayName("thông tin lỗi")]
        public virtual ErrorInfo Error { get; set; }

        [DisplayName("Stack trace")]
        public virtual List<string>? StackTrace { get; set; }
    }
}
