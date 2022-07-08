namespace TripleSix.Core.Types.Interfaces
{
    /// <summary>
    /// Dữ liệu phân trang.
    /// </summary>
    /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
    public interface IPaging<TItem>
    {
        /// <summary>
        /// Số trang.
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Kích thước trang.
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// Tổng số lượng mục.
        /// </summary>
        long Total { get; set; }

        /// <summary>
        /// Danh sách dữ liệu.
        /// </summary>
        List<TItem> Items { get; set; }
    }
}
