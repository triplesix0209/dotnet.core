namespace TripleSix.Core.Types
{
    /// <summary>
    /// Dữ liệu phân trang.
    /// </summary>
    /// <typeparam name="TItem">Loại dữ liệu.</typeparam>
    public interface IPaging<TItem>
    {
        /// <summary>
        /// Tổng số lượng mục.
        /// </summary>
        long Total { get; set; }

        /// <summary>
        /// Danh sách dữ liệu.
        /// </summary>
        List<TItem> Items { get; set; }

        /// <summary>
        /// Vị trí trang hiện tại.
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Kích thước trang dữ liệu.
        /// </summary>
        int Size { get; set; }
    }
}
