using TripleSix.Core.Helpers;

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
            : base($"Không tìm thấy {type.GetDisplayName()}")
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

        /// <inheritdoc/>
        public override int HttpCodeStatus => 404;
    }
}
