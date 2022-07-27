namespace Sample.Domain.Exceptions
{
    /// <summary>
    /// Lỗi không tìm thấy tài khoản.
    /// </summary>
    public class AccountNotFoundException : BaseException
    {
        public AccountNotFoundException()
            : base($"Không tìm thấy tài khoản")
        {
        }

        public AccountNotFoundException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 404;
    }
}
