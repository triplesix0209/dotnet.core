namespace TripleSix.Core.Persistences.Interfaces
{
    /// <summary>
    /// Interface DbContext xử lý migration.
    /// </summary>
    public interface IDbMigrationContext
    {
        /// <summary>
        /// Chạy các migrations cho database.
        /// </summary>
        /// <param name="cancellationToken">Token hỗ việc việc ngắt tiến trình xử lý.</param>
        /// <returns>Task xử lý.</returns>
        Task MigrateAsync(CancellationToken cancellationToken = default);
    }
}
