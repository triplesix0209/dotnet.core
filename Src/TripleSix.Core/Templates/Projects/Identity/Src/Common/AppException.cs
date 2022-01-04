#pragma warning disable SA1201 // ElementsMustAppearInTheCorrectOrder

using TripleSix.Core.Attributes;
using TripleSix.Core.Exceptions;

namespace ProjectCode.Common
{
    public class AppException : BaseException
    {
        public AppException(
            int httpCode = 500,
            string code = "exception",
            string message = "unexpected exception",
            object detail = null)
            : base(httpCode, code, message, detail)
        {
        }

        public AppException(
            AppExceptions error,
            object detail = null,
            params object[] args)
            : base(error, detail, args)
        {
        }
    }

    public enum AppExceptions
    {
        //[ErrorData(400, message: "phiên xác thực không hợp lệ")]
        //VerifyInvalid,
    }
}
