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
    }
}
