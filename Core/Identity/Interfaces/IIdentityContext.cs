namespace TripleSix.Core.Identity
{
    public interface IIdentityContext
    {
        /// <summary>
        /// Đã xác thực thành công.
        /// </summary>
        public bool IsVaild { get; }

        /// <summary>
        /// Id người thao tác.
        /// </summary>
        public Guid Id { get; set; }
    }
}
