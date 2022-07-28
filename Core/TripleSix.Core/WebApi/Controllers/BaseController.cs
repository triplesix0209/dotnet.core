using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ValidateInput]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Tạo <see cref="SuccessResult"/>.
        /// </summary>
        /// <returns><see cref="SuccessResult"/>.</returns>
        protected static SuccessResult SuccessResult()
        {
            return new SuccessResult();
        }

        /// <summary>
        /// Tạo <see cref="DataResult"/>.
        /// </summary>
        /// <typeparam name="TData">Loại dữ liệu.</typeparam>
        /// <param name="data">Kết quả xử lý.</param>
        /// <returns><see cref="DataResult"/>.</returns>
        protected static DataResult<TData> DataResult<TData>(TData data)
        {
            return new DataResult<TData>(data);
        }

        /// <summary>
        /// Tạo <see cref="PagingResult"/>.
        /// </summary>
        /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
        /// <param name="data">Kết quả xử lý.</param>
        /// <param name="total">Tổng số mục.</param>
        /// <param name="page">Vị trí trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns><see cref="PagingResult"/>.</returns>
        protected static PagingResult<TItem> PagingResult<TItem>(IList<TItem> data, long total, int page, int size)
        {
            return new PagingResult<TItem>(data, total, page, size);
        }

        /// <summary>
        /// Tạo <see cref="PagingResult"/>.
        /// </summary>
        /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
        /// <param name="data">Kết quả xử lý.</param>
        /// <returns><see cref="PagingResult"/>.</returns>
        protected static PagingResult<TItem> PagingResult<TItem>(IPaging<TItem> data)
        {
            return new PagingResult<TItem>(data);
        }
    }
}
