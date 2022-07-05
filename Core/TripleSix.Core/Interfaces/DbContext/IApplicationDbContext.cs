namespace TripleSix.Core.Interfaces.DbContext
{
    /// <summary>
    /// Interface DbContext của ứng dụng.
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Ghi nhận các thay đổi trên DbContext xuống database.
        /// </summary>
        /// <param name="cancellationToken">Token hỗ việc việc ngắt tiến trình xử lý.</param>
        /// <returns>Mã kết quả xử lý.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
