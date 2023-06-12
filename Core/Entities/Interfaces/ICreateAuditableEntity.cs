namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity có thể xem thời điểm được tạo ai là người thao tác.
    /// </summary>
    public interface ICreateAuditableEntity : IEntity
    {
        /// <summary>
        /// Thời gian tạo.
        /// </summary>
        DateTime? CreateAt { get; set; }

        /// <summary>
        /// Id người tạo.
        /// </summary>
        Guid? CreatorId { get; set; }
    }
}
