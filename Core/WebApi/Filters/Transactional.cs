using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.DataContext;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Bật transaction cho các request.
    /// </summary>
    public class Transactional : TypeFilterAttribute
    {
        /// <summary>
        /// Transaction cho các request.
        /// </summary>
        public Transactional()
            : base(typeof(TransactionalImplement))
        {
        }
    }

    internal class TransactionalImplement : ActionFilterAttribute
    {
        private readonly IDbDataContext _dbContext;

        public TransactionalImplement(IDbDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            var result = await next();

            if (transaction.TransactionId == _dbContext.CurrentTransaction?.TransactionId)
            {
                if (result.Exception == null) await transaction.CommitAsync();
                else await transaction.RollbackAsync();
            }
        }
    }
}
