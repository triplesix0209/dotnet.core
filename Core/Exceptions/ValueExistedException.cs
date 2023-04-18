using Microsoft.AspNetCore.Http;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi dữ liệu đã tồn tại.
    /// </summary>
    public class ValueExistedException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="ValueExistedException"/>.
        /// </summary>
        /// <param name="valueName">Tên dữ liệu.</param>
        public ValueExistedException(string valueName)
            : base($"{valueName} đã tồn tại")
        {
            ValueName = valueName;
        }

        /// <summary>
        /// Khởi tạo <see cref="ValueExistedException"/>.
        /// </summary>
        /// <param name="valueName">Tên dữ liệu.</param>
        /// <param name="message"><inheritdoc/></param>
        public ValueExistedException(string valueName, string message)
            : base(message)
        {
            ValueName = valueName;
        }

        /// <summary>
        /// Tên dữ liệu.
        /// </summary>
        public virtual string ValueName { get; }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 500;

        /// <inheritdoc/>
        public override ErrorResult ToErrorResult(HttpContext? httpContext = null)
        {
            return new ErrorResult(HttpCodeStatus, Code, Message, new { ValueName });
        }
    }
}
