namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity có thể xem thời điểm được sửa cuối và ai là người thao tác.
    /// </summary>
    public interface IUpdateAuditableEntity : IEntity
    {
        /// <summary>
        /// Thời gian chỉnh sửa cuối.
        /// </summary>
        DateTime? UpdateAt { get; set; }

        /// <summary>
        /// Id người chỉnh sửa cuối.
        /// </summary>
        Guid? UpdatorId { get; set; }
    }
}
