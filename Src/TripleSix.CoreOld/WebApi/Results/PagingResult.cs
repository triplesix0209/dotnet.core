using System.Collections.Generic;
using System.Linq;
using TripleSix.CoreOld.Dto;

namespace TripleSix.CoreOld.WebApi.Results
{
    public class PagingResult<TItem> : DataResult<IList<TItem>, PagingMeta>
    {
        public PagingResult()
        {
            Meta = new PagingMeta { Total = 0, Page = 0, Size = 0 };
            Data = new List<TItem>();
        }

        public PagingResult(params TItem[] items)
        {
            Meta = new PagingMeta { Total = items.Length, Page = items.Length, Size = items.Length };
            Data = items.ToList();
        }

        public PagingResult(
            IList<TItem> data,
            long total,
            int page,
            int? size = null)
        {
            Meta = new PagingMeta { Total = total, Page = page, Size = size ?? data.Count };
            Data = data;
        }

        public PagingResult(IPaging<TItem> data)
        {
            Meta = new PagingMeta { Total = data.Total, Page = data.Page, Size = data.Size };
            Data = data.Items;
        }
    }
}