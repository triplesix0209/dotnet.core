using System.Threading.Tasks;
using ClosedXML.Excel;
using TripleSix.AutoAdmin.Dto;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;

namespace TripleSix.AutoAdmin.Interfaces
{
    /// <summary>
    /// service có export dữ liệu.
    /// </summary>
    /// <typeparam name="TEntity">Entity mà service quản lý.</typeparam>
    public interface IExportService<TEntity> : IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        Task<XLWorkbook> Export(IIdentity identity, IModelDataDto[] data, ExportInputDto config);
    }
}