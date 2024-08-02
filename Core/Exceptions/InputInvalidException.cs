using Microsoft.AspNetCore.Http;
using TripleSix.Core.Helpers;
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
        /// <param name="errorCode">Mã lỗi.</param>
        /// <param name="errorMessage">Mô tả lỗi.</param>
        public InputInvalidException(
            string fieldName,
            string errorMessage = "Bị sai dữ liệu",
            string errorCode = "runtime_validator")
            : base($"Dữ liệu đầu vào bị thiếu hoặc sai")
        {
            Items = new InputInvalidItem[]
            {
                new() { FieldName = fieldName, ErrorCode = errorCode, ErrorMessage = errorMessage },
            };
        }

        /// <summary>
        /// Khởi tạo <see cref="InputInvalidException"/>.
        /// </summary>
        /// <param name="items">Danh sách lỗi.</param>
        public InputInvalidException(IEnumerable<InputInvalidItem> items)
            : base($"Dữ liệu đầu vào bị thiếu hoặc sai")
        {
            Items = items.ToArray();
        }

        /// <summary>
        /// Khởi tạo <see cref="InputInvalidException"/>.
        /// </summary>
        /// <param name="items">Danh sách mục lỗi.</param>
        public InputInvalidException(params InputInvalidItem[] items)
            : base($"Dữ liệu đầu vào bị thiếu hoặc sai")
        {
            Items = items;
        }

        /// <summary>
        /// Danh sách các mục lỗi.
        /// </summary>
        public virtual InputInvalidItem[] Items { get; }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 400;

        /// <inheritdoc/>
        public override ErrorResult ToErrorResult(HttpContext? httpContext = null)
        {
            var message = Message;
            var fieldErrors = Items.Where(x => x.ErrorCode != "model_validator");
            if (fieldErrors.Any())
                message += ": " + fieldErrors.Select(x => x.ErrorMessage).ToString("; ");

            return new ErrorResult(HttpCodeStatus, Code, message, Items, StackTrace);
        }
    }

    /// <summary>
    /// Thông tin lỗi nhập liệu.
    /// </summary>
    public class InputInvalidItem
    {
        /// <summary>
        /// Tên trường dữ liệu.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Mã lỗi.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Mô tả lỗi.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
