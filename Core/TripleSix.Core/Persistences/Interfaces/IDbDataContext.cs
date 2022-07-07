namespace TripleSix.Core.Persistences.Interfaces
{
    /// <summary>
    /// Interface DbContext xử lý dữ liệu.
    /// </summary>
    public interface IDbDataContext
    {
        /// <summary>
        /// Ghi nhận các thay đổi trên DbContext xuống database.
        /// </summary>
        /// <param name="cancellationToken">Token hỗ việc việc ngắt tiến trình xử lý.</param>
        /// <returns>Mã kết quả xử lý.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
