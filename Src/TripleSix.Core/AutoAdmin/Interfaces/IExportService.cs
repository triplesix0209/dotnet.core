using System.Threading.Tasks;
using ClosedXML.Excel;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;

namespace TripleSix.Core.AutoAdmin
{
    public interface IExportService<TEntity> : IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        Task<XLWorkbook> Export(IIdentity identity, IModelDataDto[] data, ExportInputDto config);
    }
}