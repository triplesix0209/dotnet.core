namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi không tìm thấy entity.
    /// </summary>
    public class EntityNotFoundException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="EntityNotFoundException"/>.
        /// </summary>
        /// <param name="entityType">Loại entity.</param>
        public EntityNotFoundException(Type entityType)
            : base($"Không tìm thấy {entityType.Name}")
        {
            EntityType = entityType;
        }

        /// <summary>
        /// Khởi tạo <see cref="EntityNotFoundException"/>.
        /// </summary>
        /// <param name="entityType">Loại entity.</param>
        /// <param name="message"><inheritdoc/></param>
        public EntityNotFoundException(Type entityType, string message)
            : base(message)
        {
            EntityType = entityType;
        }

        /// <summary>
        /// Loại entity.
        /// </summary>
        public virtual Type EntityType { get; }

        /// <inheritdoc/>
        public override int HttpCodeStatus => 404;
    }
}
