namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Interface Context xử lý migration.
    /// </summary>
    public interface IDbMigrationContext
    {
        /// <summary>
        /// Apply all migrations.
        /// </summary>
        void Migrate();

        /// <summary>
        /// Apply all migrations.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous migrate operation.
        /// </returns>
        Task MigrateAsync(CancellationToken cancellationToken = default);
    }
}
