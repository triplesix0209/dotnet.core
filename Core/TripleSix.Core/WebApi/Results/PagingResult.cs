using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    public class PagingResult<TItem> : BaseResult<PagingMeta>
    {
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

        public PagingResult(IPaging<TItem> data)
            : base(true, 200)
        {
            Meta = new PagingMeta { Success = true, Total = data.Total, Page = data.Page, Size = data.Size };
            Data = data.Items;
        }

        [JsonProperty(Order = -9)]
        [DisplayName("kết quả xử lý")]
        public virtual IEnumerable<TItem> Data { get; protected set; }
    }
}
