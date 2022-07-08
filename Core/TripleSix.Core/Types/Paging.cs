using TripleSix.Core.Types.Interfaces;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Dữ liệu phân trang.
    /// </summary>
    /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
    public class Paging<TItem>
        : IPaging<TItem>
    {
        /// <summary>
        /// Khởi tạo <see cref="Paging{TItem}"/> class.
        /// </summary>
        /// <param name="items">Danh sách dữ liệu.</param>
        /// <param name="total">Tổng số lượng mục.</param>
        /// <param name="page">Số trang.</param>
        /// <param name="size">Kích thước trang.</param>
        public Paging(List<TItem> items, long total, int page, int size)
        {
            Total = total;
            Items = items;
            Page = page;
            Size = size;
        }

        /// <inheritdoc/>
        public long Total { get; set; }

        /// <inheritdoc/>
        public List<TItem> Items { get; set; }

        /// <inheritdoc/>
        public int Page { get; set; }

        /// <inheritdoc/>
        public int Size { get; set; }
    }
}
