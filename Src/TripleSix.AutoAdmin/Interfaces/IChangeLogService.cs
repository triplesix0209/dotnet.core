using System;
using System.Threading.Tasks;
using TripleSix.AutoAdmin.Dto;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;

namespace TripleSix.AutoAdmin.Interfaces
{
    /// <summary>
    /// Service có ghi nhận changelog.
    /// </summary>
    /// <typeparam name="TEntity">Entity mà service quản lý.</typeparam>
    public interface IChangeLogService<TEntity> : IModelService<TEntity>
        where TEntity : class, IModelEntity
    {
        /// <summary>
        /// lấy phân trang danh sách changelog của một đối tượng.
        /// </summary>
        /// <param name="identity">identity phiên xử lý.</param>
        /// <param name="id">id của đối tượng cần lấy.</param>
        /// <param name="page">số trang.</param>
        /// <param name="size">kích thước trang.</param>
        /// <returns>danh sách phân trang các changelog.</returns>
        Task<IPaging<ObjectLogDto>> GetPageChangeLog(IIdentity identity, Guid id, int page, int size = 10);

        /// <summary>
        /// lấy chi tiết changelog.
        /// </summary>
        /// <param name="identity">identity phiên xử lý.</param>
        /// <param name="objectLogId">id changelog.</param>
        /// <returns>thông tin changelog.</returns>
        Task<ObjectLogDto> GetChangeLog(IIdentity identity, Guid objectLogId);

        /// <summary>
        /// ghi nhận changelog của đối tượng.
        /// </summary>
        /// <param name="identity">identity phiên xử lý.</param>
        /// <param name="id">id của đối tượng cần ghi nhận.</param>
        /// <param name="beforeData">thông tin trước khi chỉnh sửa.</param>
        /// <returns>kết quả tiến trình xử lý.</returns>
        Task WriteChangeLog(IIdentity identity, Guid id, string beforeData = null);
    }
}