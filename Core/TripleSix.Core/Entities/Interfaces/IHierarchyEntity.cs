namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity có thể xem thời điểm được tạo, sửa cuối và ai là người thao tác.
    /// </summary>
    /// <typeparam name="TEntity">Entity cha.</typeparam>
    public interface IHierarchyEntity<TEntity> : IStrongEntity
        where TEntity : IStrongEntity
    {
        /// <summary>
        /// Id cha.
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Cấp con.
        /// </summary>
        int HierarchyLevel { get; set; }

        /// <summary>
        /// Mục cha.
        /// </summary>
        TEntity? Parent { get; set; }

        /// <summary>
        /// Danh sách mục con.
        /// </summary>
        List<TEntity>? Childs { get; set; }
    }
}
