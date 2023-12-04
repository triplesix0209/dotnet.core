using System.ComponentModel;

namespace TripleSix.CoreOld.Dto
{
    public class Paging<TItem>
        : IPaging<TItem>
    {
        [DisplayName("tổng số mục")]
        public virtual long Total { get; set; }

        [DisplayName("vị trí trang")]
        public virtual int Page { get; set; }

        [DisplayName("kích thước trang")]
        public virtual int Size { get; set; }

        [DisplayName("danh sách dữ liệu")]
        public virtual TItem[] Items { get; set; }
    }
}