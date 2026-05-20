#pragma warning disable SA1649 // File name should match first type name

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TripleSix.Core.DataContext;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Transactional Options.
    /// </summary>
    public static class TransactionalOptions
    {
        /// <summary>
        /// Default DbContext Type.
        /// </summary>
        public static Type[] DefaultDbContextTypes { get; set; } = [typeof(IDbDataContext)];
    }

    /// <summary>
    /// Bật transaction cho các request.
    /// </summary>
    public class Transactional : TypeFilterAttribute
    {
        /// <summary>
        /// Transaction cho các request.
        /// </summary>
        /// <param name="dbContextTypes">Các kiểu DbContext cần chạy transaction (ví dụ: typeof(IApplicationDbContext)).</param>
        public Transactional(params Type[] dbContextTypes)
            : base(typeof(TransactionalImplement))
        {
            DbContextTypes = dbContextTypes.Length > 0 ? dbContextTypes : TransactionalOptions.DefaultDbContextTypes;
            Arguments = [DbContextTypes];
        }

        /// <summary>
        /// Các DbContext cần chạy transaction.
        /// </summary>
        public Type[] DbContextTypes { get; }
    }

    internal class TransactionalImplement : ActionFilterAttribute
    {
        private readonly Type[] _dbContextTypes;

        public TransactionalImplement(Type[] dbContextTypes)
        {
            _dbContextTypes = dbContextTypes;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var dbContexts = _dbContextTypes
                .Select(type => (IDbDataContext)context.HttpContext.RequestServices.GetRequiredService(type))
                .Distinct()
                .ToList();

            var transactions = new List<IDbContextTransaction>();
            try
            {
                foreach (var db in dbContexts)
                    transactions.Add(await db.BeginTransactionAsync());

                var result = await next();
                if (result.Exception == null)
                {
                    foreach (var transaction in transactions)
                        await transaction.CommitAsync();
                }
                else
                {
                    foreach (var transaction in transactions)
                        await transaction.RollbackAsync();
                }
            }
            finally
            {
                foreach (var transaction in transactions)
                    await transaction.DisposeAsync();
            }
        }
    }
}
