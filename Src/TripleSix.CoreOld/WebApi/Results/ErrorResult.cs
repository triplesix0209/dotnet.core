using System.ComponentModel;

namespace TripleSix.CoreOld.WebApi.Results
{
    public class ErrorResult : BaseResult<ErrorMeta>
    {
        public ErrorResult(
            int statusCode = 500,
            string code = "exception",
            string message = "internal server error")
            : base(statusCode)
        {
            Error = new BaseError { Code = code, Message = message };
        }

        [DisplayName("thông tin lỗi")]
        public virtual BaseError Error { get; protected set; }
    }
}