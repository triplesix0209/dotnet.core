using System.ComponentModel;

namespace TripleSix.Core.WebApi.Results
{
    public class ErrorResult<TData> : BaseResult<ErrorMeta>
    {
        public ErrorResult(
            int statusCode = 500,
            string code = "exception",
            string message = "internal server error",
            TData data = default)
            : base(statusCode)
        {
            Error = new DataError<TData> { Code = code, Message = message, Data = data };
        }

        [DisplayName("thông tin lỗi")]
        public virtual DataError<TData> Error { get; protected set; }
    }
}