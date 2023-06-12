namespace TripleSix.Core.Entities
{
    /// <summary>
    /// Entity độc lập.
    /// </summary>
    public interface IStrongEntity :
        IIdentifiableEntity,
        ISoftDeletableEntity,
        ICreateAuditableEntity,
        IUpdateAuditableEntity
    {
    }
}
