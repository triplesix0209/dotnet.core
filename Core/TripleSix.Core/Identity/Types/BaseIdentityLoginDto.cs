using TripleSix.Core.Types;

namespace TripleSix.Core.Identity
{
    /// <summary>
    /// Thông tin đăng nhập.
    /// </summary>
    public class BaseIdentityLoginDto : DataDto
    {
        /// <summary>
        /// Tên đăng nhập.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Mật khẩu.
        /// </summary>
        public string? Password { get; set; }
    }
}
