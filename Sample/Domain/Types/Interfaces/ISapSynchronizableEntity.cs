namespace Sample.Domain.Types
{
    public interface ISapSynchronizableEntity : IStrongEntity
    {
        string SapCode { get; set; }
    }
}
