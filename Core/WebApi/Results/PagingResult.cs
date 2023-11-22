using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Paging result.
    /// </summary>
    /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
    public class PagingResult<TItem> : BaseResult<PagingMeta>
    {
        /// <summary>
        /// Paging result.
        /// </summary>
        /// <param name="data">Danh sách dữ liệu.</param>
        /// <param name="total">Tổng số mục.</param>
        /// <param name="page">Vị trí trang.</param>
        /// <param name="size">Kích thước trang.</param>
        public PagingResult(
            IEnumerable<TItem> data,
            long total,
            int page,
            int size)
            : base(true, 200)
        {
            Meta = new PagingMeta { Success = true, Total = total, Page = page, Size = size };
            Data = data;
        }

        /// <summary>
        /// Paging result.
        /// </summary>
        /// <param name="data">Dữ liệu.</param>
        public PagingResult(IPaging<TItem> data)
            : base(true, 200)
        {
            Meta = new PagingMeta { Success = true, Total = data.Total, Page = data.Page, Size = data.Size };
            Data = data.Items;
        }

        /// <summary>
        /// Dữ liệu.
        /// </summary>
        [JsonProperty(Order = -9)]
        [DisplayName("Dữ liệu")]
        public virtual IEnumerable<TItem> Data { get; protected set; }
    }
}
