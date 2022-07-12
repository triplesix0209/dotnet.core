namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi không tìm thấy entity.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Khởi tạo <see cref="EntityNotFoundException"/>.
        /// </summary>
        /// <param name="entityType">Loại entity.</param>
        public EntityNotFoundException(Type entityType)
            : base($"{entityType.Name} not found")
        {
            EntityType = entityType;
            Query = null;
        }

        /// <summary>
        /// Khởi tạo <see cref="EntityNotFoundException"/>.
        /// </summary>
        /// <param name="entityType">Loại entity.</param>
        /// <param name="query">Câu query.</param>
        public EntityNotFoundException(Type entityType, IQueryable query)
            : base($"{entityType.Name} not found")
        {
            EntityType = entityType;
            Query = query;
        }

        /// <summary>
        /// Loại entity.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Câu query.
        /// </summary>
        public IQueryable? Query { get; }
    }
}
