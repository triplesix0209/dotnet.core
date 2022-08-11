using Microsoft.AspNetCore.Http;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi nhập liệu.
    /// </summary>
    public class InputInvalidException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="InputInvalidException"/>.
        /// </summary>
        /// <param name="fieldName">Tên trường dữ liệu.</param>
        public InputInvalidException(string fieldName)
            : base($"Dữ liệu {fieldName} bị sai")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Khởi tạo <see cref="InputInvalidException"/>.
        /// </summary>
        /// <param name="fieldName">Tên trường dữ liệu.</param>
        /// <param name="message"><inheritdoc/></param>
        public InputInvalidException(string fieldName, string message)
            : base($"{message}")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Tên trường dữ liệu.
        /// </summary>
        public virtual string FieldName { get; }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 400;

        /// <inheritdoc/>
        public override ErrorResult ToErrorResult(HttpContext? httpContext = null)
        {
            return new ErrorResult(HttpCodeStatus, Code, Message, new { FieldName });
        }
    }
}
