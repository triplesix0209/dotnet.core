using System.ComponentModel;

namespace TripleSix.Core.WebApi
{
    public class ErrorResult : BaseResult<BaseMeta>
    {
        public ErrorResult(
            int httpStatusCode = 500,
            string code = "exception",
            string message = "internal server error")
            : base(false, httpStatusCode)
        {
            Error = new BaseError(code, message);
        }

        [DisplayName("thông tin lỗi")]
        public virtual BaseError Error { get; set; }
    }
}
