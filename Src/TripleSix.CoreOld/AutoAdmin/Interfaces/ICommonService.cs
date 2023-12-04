using TripleSix.CoreOld.Entities;

namespace TripleSix.CoreOld.AutoAdmin
{
    public interface ICommonService<TEntity> :
        IChangeLogService<TEntity>,
        IExportService<TEntity>
        where TEntity : class, IModelEntity
    {
    }
}
