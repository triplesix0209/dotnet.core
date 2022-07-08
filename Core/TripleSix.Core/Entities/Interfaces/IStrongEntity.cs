namespace TripleSix.Core.Entities.Interfaces
{
    /// <summary>
    /// Entity độc lập.
    /// </summary>
    public interface IStrongEntity
        : IIdentifiableEntity, ISoftDeletableEntity, IAuditableEntity, IHasCodeEntity
    {
    }
}
