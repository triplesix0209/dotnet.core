using TripleSix.Core.Entities;

namespace TripleSix.AutoAdmin.Interfaces
{
    public interface ICommonService<TEntity> :
        IChangeLogService<TEntity>,
        IExportService<TEntity>
        where TEntity : class, IModelEntity
    {
    }
}
