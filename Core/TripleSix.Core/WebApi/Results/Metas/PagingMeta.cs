using System.ComponentModel;

namespace TripleSix.Core.WebApi
{
    public class PagingMeta : BaseMeta
    {
        [DisplayName("tổng số item")]
        public virtual long Total { get; set; }

        [DisplayName("vị trí trang hiện tại")]
        public virtual long Page { get; set; }

        [DisplayName("kích thước trang dữ liệu")]
        public virtual int Size { get; set; }

        [DisplayName("tổng số trang")]
        public virtual long PageCount => Size == 0 ? 0 : (long)Math.Ceiling((decimal)Total / Size);

        [DisplayName("còn trang kế tiếp?")]
        public virtual bool CanNext => Page < PageCount;

        [DisplayName("có trang trước?")]
        public virtual bool CanPrev => Page > 1;
    }
}
