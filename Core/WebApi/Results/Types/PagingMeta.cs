using System.ComponentModel;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Paging metadata.
    /// </summary>
    public class PagingMeta : BaseMeta
    {
        /// <summary>
        /// Tổng số item.
        /// </summary>
        [DisplayName("Tổng số item")]
        public virtual long Total { get; set; }

        /// <summary>
        /// Vị trí trang hiện tại.
        /// </summary>
        [DisplayName("Vị trí trang hiện tại")]
        public virtual long Page { get; set; }

        /// <summary>
        /// Kích thước trang dữ liệu.
        /// </summary>
        [DisplayName("Kích thước trang dữ liệu")]
        public virtual int Size { get; set; }

        /// <summary>
        /// Tổng số trang.
        /// </summary>
        [DisplayName("Tổng số trang")]
        public virtual long PageCount => Size == 0 ? 0 : (long)Math.Ceiling((decimal)Total / Size);

        /// <summary>
        /// Còn trang kế tiếp?.
        /// </summary>
        [DisplayName("Còn trang kế tiếp?")]
        public virtual bool CanNext => Page < PageCount;

        /// <summary>
        /// Có trang trước?.
        /// </summary>
        [DisplayName("Có trang trước?")]
        public virtual bool CanPrev => Page > 1;
    }
}
