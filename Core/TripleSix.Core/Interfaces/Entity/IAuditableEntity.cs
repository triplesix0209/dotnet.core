namespace TripleSix.Core.Interfaces.Entity
{
    /// <summary>
    /// Entity có thể xem thời điểm được tạo, sửa cuối và ai là người thao tác.
    /// </summary>
    public interface IAuditableEntity : IEntity
    {
        /// <summary>
        /// Thời gian tạo.
        /// </summary>
        DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Id người tạo.
        /// </summary>
        Guid? CreatorId { get; set; }

        /// <summary>
        /// Thời gian chỉnh sửa cuối.
        /// </summary>
        DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// Id người chỉnh sửa cuối.
        /// </summary>
        Guid? UpdatorId { get; set; }
    }
}
