namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity có thể soft delete (chỉ đánh dấu đã xóa, chứ không thật sự bị xóa khỏi database).
    /// </summary>
    public interface ISoftDeletableEntity : IEntity
    {
        /// <summary>
        /// Thời gian xóa.
        /// </summary>
        DateTime? DeleteAt { get; set; }
    }
}
