namespace TripleSix.Core.Interfaces.Entity
{
    /// <summary>
    /// Entity có thể soft delete (chỉ đánh dấu đã xóa, chứ không thật sự bị xóa mất khỏi database).
    /// </summary>
    public interface ISoftDeletableEntity : IEntity
    {
        /// <summary>
        /// Đã bị đã xóa.
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
