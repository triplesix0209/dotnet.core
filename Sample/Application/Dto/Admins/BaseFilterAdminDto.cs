namespace Sample.Application.Dto.Admins
{
    public abstract class BaseFilterAdminDto<TEntity> : BaseEntityQueryDto<TEntity>,
        IPagingInput
        where TEntity : IStrongEntity
    {
        /// <inheritdoc/>
        [DisplayName("Vị trí trang")]
        public int Page { get; set; } = 1;

        /// <inheritdoc/>
        [DisplayName("Kích thước trang")]
        public int Size { get; set; } = 10;
    }
}
