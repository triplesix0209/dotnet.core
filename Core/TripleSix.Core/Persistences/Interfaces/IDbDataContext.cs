using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TripleSix.Core.Persistences
{
    /// <summary>
    /// Interface DbContext xử lý dữ liệu.
    /// </summary>
    public interface IDbDataContext
    {
        /// <summary>
        /// Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous transaction initialization. The task result contains a Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction that represents the started transaction.
        /// </returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
