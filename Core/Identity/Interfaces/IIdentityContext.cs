namespace TripleSix.Core.Identity
{
    public interface IIdentityContext
    {
        /// <summary>
        /// Id người thao tác.
        /// </summary>
        public Guid? Id { get; set; }
    }
}
