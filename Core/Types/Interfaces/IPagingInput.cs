namespace TripleSix.Core.Types
{
    /// <summary>
    /// Tham số phân trang.
    /// </summary>
    public interface IPagingInput
    {
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
