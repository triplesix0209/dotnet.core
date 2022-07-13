using TripleSix.Core.Helpers;

namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi cơ bản.
    /// </summary>
    public class BaseException : Exception
    {
        /// <summary>
        /// Khởi tạo <see cref="BaseException"/>.
        /// </summary>
        /// <param name="message">Mô tả lỗi.</param>
        public BaseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Mã lỗi.
        /// </summary>
        public virtual string Code
        {
            get
            {
                if (GetType() == typeof(BaseException))
                    return "exception";

                var code = GetType().Name;
                if (code.EndsWith("Exception"))
                    code = code[..^9];
                code = code.ToSnakeCase();
                return code;
            }
        }

        /// <summary>
        /// Mã số trạng thái HTTP.
        /// </summary>
        public virtual int HttpCodeStatus => 500;
    }
}
