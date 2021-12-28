using System;
using System.ComponentModel;

namespace TripleSix.Core.WebApi.Results
{
    public class PagingMeta : BaseMeta
    {
        public override bool Success { get; set; } = true;

        [DisplayName("tổng số item")]
        public virtual long Total { get; set; }

        [DisplayName("vị trí trang hiện tại")]
        public virtual long Page { get; set; }

        [DisplayName("kích thước một trang dữ liệu")]
        public virtual int Size { get; set; }

        [DisplayName("tổng số trang")]
        public virtual long PageCount => (long)Math.Ceiling((decimal)Total / Size);

        [DisplayName("còn trang kế tiếp?")]
        public virtual bool CanNext => Page < PageCount;

        [DisplayName("có trang trước?")]
        public virtual bool CanPrev => Page > 1;
    }
}