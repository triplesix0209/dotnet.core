using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base controller.
    /// </summary>
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ValidateInput]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Auto ampper.
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Tạo success result.
        /// </summary>
        /// <returns>Success result.</returns>
        protected static SuccessResult SuccessResult()
        {
            return new SuccessResult();
        }

        /// <summary>
        /// Tạo data result.
        /// </summary>
        /// <typeparam name="TData">Loại dữ liệu.</typeparam>
        /// <param name="data">Kết quả xử lý.</param>
        /// <returns>Data result.</returns>
        protected static DataResult<TData> DataResult<TData>(TData data)
        {
            return new DataResult<TData>(data);
        }

        /// <summary>
        /// Tạo Paging result.
        /// </summary>
        /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
        /// <param name="data">Kết quả xử lý.</param>
        /// <param name="total">Tổng số mục.</param>
        /// <param name="page">Vị trí trang.</param>
        /// <param name="size">Kích thước trang.</param>
        /// <returns>Paging result.</returns>
        protected static PagingResult<TItem> PagingResult<TItem>(IList<TItem> data, long total, int page, int size)
        {
            return new PagingResult<TItem>(data, total, page, size);
        }

        /// <summary>
        /// Tạo Paging result.
        /// </summary>
        /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
        /// <param name="data">Kết quả xử lý.</param>
        /// <returns>Paging result.</returns>
        protected static PagingResult<TItem> PagingResult<TItem>(IPaging<TItem> data)
        {
            return new PagingResult<TItem>(data);
        }
    }
}
