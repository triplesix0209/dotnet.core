using TripleSix.Core.Exceptions;

namespace Sample.Domain.Exceptions
{
    /// <summary>
    /// Lỗi phiên đăng nhập.
    /// </summary>
    public class SessionInvalidException : BaseException
    {
        public SessionInvalidException()
            : base($"Không tìm thấy tài khoản")
        {
        }

        public SessionInvalidException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 401;
    }
}
