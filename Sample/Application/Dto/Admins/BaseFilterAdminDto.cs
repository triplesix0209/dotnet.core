namespace Sample.Application.Dto.Admins
{
    public abstract class BaseFilterAdminDto<TEntity> : BaseEntityQueryDto<TEntity>
        where TEntity : IStrongEntity
    {
    }
}
