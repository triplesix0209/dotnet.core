using System.Threading.Tasks;
using ClosedXML.Excel;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Services;

namespace TripleSix.CoreOld.AutoAdmin
{
    public interface IExportService<TEntity> : IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        Task<XLWorkbook> Export(IIdentity identity, IModelDataDto[] data, ExportInputDto config);
    }
}