#pragma warning disable SA1201 // ElementsMustAppearInTheCorrectOrder


namespace Sample.Common
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
        [ErrorData(400, message: "phiên xác thực không hợp lệ")]
        VerifyInvalid,

        [ErrorData(400, message: "mật khẩu của bạn quá yếu, phải ít nhất 6 ký tự")]
        PasswordTooWeak,

        [ErrorData(400, message: "e-mail của tài khoản không hợp lệ")]
        AccountEmailInvalid,

        [ErrorData(400, message: "tài khoản chưa được xác thực")]
        AccountNeedVerifed,

        [ErrorData(400, message: "tài khoản chưa được xác thực, chúng tôi đã gửi e-mail xác thực cho bạn, xin vui lòng kiểm tra mail")]
        AccountNeedVerifedAndWeAlreadySentVerify,

        [ErrorData(400, message: "tài khoản đã được xác thực")]
        AccountAlreadyVerifed,

        [ErrorData(500, message: "phương thức đăng nhập {0} đã tồn tại")]
        AccountAuthExisted,

        [ErrorData(400, message: "tên đăng nhập hoặc mật khẩu không chính xác")]
        AccountAuthInvalid,

        [ErrorData(401, message: "phiên đăng nhập bị lỗi hoặc đã hết hạn")]
        SessionInvalid,

        [ErrorData(404, message: "không tìm thấy tài khoản")]
        AccountNotFound,

        [ErrorData(403, message: "tài khoản bị khóa")]
        AccountInactive,

        [ErrorData(403, message: "phương thức xác thực của tài khoản bị khóa hoặc chưa có")]
        AccountAuthInactiveOrNotFound,

        [ErrorData(400, message: "mật khẩu cũ không chính xác")]
        OldPasswordInvalid,

        [ErrorData(400, message: "username '{0}' đã tồn tại")]
        UsernameIsExisted,

        [ErrorData(500, message: "không được phép tạo tài khoản root")]
        CannotCreateRootAccount,

        [ErrorData(500, message: "tài khoản root không thể bị chỉnh sửa")]
        RootAccountUnmodified,
    }
}
