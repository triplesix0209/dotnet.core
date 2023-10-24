using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TripleSix.Core.DataContext
{
    /// <summary>
    /// Interface Context xử lý dữ liệu.
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
        /// starts a new transaction.
        /// </summary>
        /// <returns>
        /// Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction that represents the started transaction.
        /// </returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Asynchronously starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous transaction initialization. The task result contains a Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction that represents the started transaction.
        /// </returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="autoAudit">Auto set value for CreateAt, UpdateAt, CreatorId, UpdatorId.</param>
        /// <returns>
        /// the number of state entries written to the database.
        /// </returns>
        int SaveChanges(bool autoAudit = true);

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="autoAudit">Auto set value for CreateAt, UpdateAt, CreatorId, UpdatorId.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(bool autoAudit = true, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the current <see cref="IDbContextTransaction" /> being used by the context, or null
        ///     if no transaction is in use.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This property is null unless one of <see cref="BeginTransaction" />,
        ///         <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.BeginTransaction" />, or
        ///         <see cref="O:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.UseTransaction" />
        ///         has been called.
        ///         No attempt is made to obtain a transaction from the current DbConnection or similar.
        ///     </para>
        ///     <para>
        ///         For relational databases, the underlying <see cref="DbTransaction" /> can be obtained using
        ///         <see cref="M:Microsoft.EntityFrameworkCore.Storage.DbContextTransactionExtensions.GetDbTransaction" />
        ///         on the returned <see cref="IDbContextTransaction" />.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-transactions">Transactions in EF Core</see> for more information and examples.
        ///     </para>
        /// </remarks>
        IDbContextTransaction? CurrentTransaction { get; }
    }
}
