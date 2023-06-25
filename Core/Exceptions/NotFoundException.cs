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
        /// <param name="entityType">Loại entity.</param>
        public NotFoundException(Type entityType)
            : base($"Không tìm thấy {entityType.Name}")
        {
            EntityType = entityType;
        }

        /// <summary>
        /// Khởi tạo <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="entityType">Loại entity.</param>
        /// <param name="message"><inheritdoc/></param>
        public NotFoundException(Type entityType, string message)
            : base(message)
        {
            EntityType = entityType;
        }

        /// <summary>
        /// Loại entity.
        /// </summary>
        public virtual Type EntityType { get; }

        public override int HttpCodeStatus => 404;
    }
}
