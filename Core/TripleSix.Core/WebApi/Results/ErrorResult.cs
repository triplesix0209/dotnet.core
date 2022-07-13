using System.ComponentModel;

namespace TripleSix.Core.WebApi
{
    public class ErrorResult : BaseResult<BaseMeta>
    {
        public ErrorResult(
            int httpStatusCode = 500,
            string code = "exception",
            string message = "internal server error",
            object? data = null)
            : base(false, httpStatusCode)
        {
            Error = new ErrorInfo(code, message, data);
            Detail = new List<string>();
        }

        [DisplayName("thông tin lỗi")]
        public virtual ErrorInfo Error { get; set; }

        [DisplayName("Chi tiết lỗi")]
        public virtual List<string> Detail { get; set; }
    }
}
