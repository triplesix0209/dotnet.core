namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi không tìm thấy entity.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityType">Type of entity.</param>
        /// <param name="query">Queries against a specific data source.</param>
        public EntityNotFoundException(Type entityType, IQueryable? query)
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
        /// Câu quyery.
        /// </summary>
        public IQueryable? Query { get; }
    }
}
