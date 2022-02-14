using TripleSix.Core.Entities;

namespace TripleSix.Core.AutoAdmin
{
    public interface ICommonService<TEntity> :
        IChangeLogService<TEntity>,
        IExportService<TEntity>
        where TEntity : class, IModelEntity
    {
    }
}
