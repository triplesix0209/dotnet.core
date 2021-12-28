using System.Collections.Generic;
using CpTech.Core.Dto;

namespace CpTech.Core.WebApi.Results
{
    public class PagingResult<TItem> : DataResult<IList<TItem>, PagingMeta>
    {
        public PagingResult(
            IList<TItem> data,
            long total,
            int page,
            int? size)
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