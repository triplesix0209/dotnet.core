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
            : base($"{fieldName} bị sai dữ liệu đầu vào")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Khởi tạo <see cref="InputInvalidException"/>.
        /// </summary>
        /// <param name="fieldName">Tên trường dữ liệu.</param>
        /// <param name="message"><inheritdoc/></param>
        public InputInvalidException(string fieldName, string message)
            : base($"{fieldName}: {message}")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Tên trường dữ liệu.
        /// </summary>
        public virtual string FieldName { get; }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 400;
    }
}
