namespace TripleSix.Core.Interfaces.Entity
{
    /// <summary>
    /// Entity có mã Id để định danh.
    /// </summary>
    public interface IIdentifiableEntity : IEntity
    {
        /// <summary>
        /// Mã Id.
        /// </summary>
        Guid Id { get; set; }
    }
}
