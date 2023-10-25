namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi không tìm thấy entity.
    /// </summary>
    public class NotFoundException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="type">Loại đối tượng.</param>
        public NotFoundException(Type type)
            : base($"Không tìm thấy {type.Name}")
        {
        }

        /// <summary>
        /// Khởi tạo <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="name">Tên đối tượng.</param>
        public NotFoundException(string name)
            : base($"Không tìm thấy {name}")
        {
        }

        public override int HttpCodeStatus => 404;
    }
}
