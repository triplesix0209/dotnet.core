namespace Sample.Application.Dto.Admins
{
    public abstract class BaseFilterAdminDto<TEntity> : BaseQueryDto<TEntity>,
        IPagingInput
        where TEntity : IStrongEntity
    {
        [DisplayName("Vị trí trang")]
        public int Page { get; set; } = 1;

        [DisplayName("Kích thước trang")]
        public int Size { get; set; } = 10;
    }
}
