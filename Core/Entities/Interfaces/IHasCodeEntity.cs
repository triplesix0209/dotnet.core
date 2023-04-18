namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity có mã số phục vụ cho việc hiển thị.
    /// </summary>
    public interface IHasCodeEntity : IEntity
    {
        /// <summary>
        /// Mã số.
        /// </summary>
        string? Code { get; set; }
    }
}
