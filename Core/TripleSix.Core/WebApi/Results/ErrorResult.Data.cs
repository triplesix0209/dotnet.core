using System.ComponentModel;

namespace TripleSix.Core.WebApi
{
    public class ErrorResult<TData> : BaseResult<BaseMeta>
    {
        public ErrorResult(
            int httpStatusCode = 500,
            string code = "exception",
            string message = "internal server error",
            TData? data = default)
            : base(false, httpStatusCode)
        {
            Error = new DataError<TData>(code, message, data);
        }

        [DisplayName("thông tin lỗi")]
        public virtual DataError<TData> Error { get; set; }
    }
}
