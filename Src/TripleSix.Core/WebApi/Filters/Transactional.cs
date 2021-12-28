using System.Threading.Tasks;
using TripleSix.Core.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TripleSix.Core.WebApi.Filters
{
    public class Transactional : TypeFilterAttribute
    {
        public Transactional()
            : base(typeof(TransactionalImplement))
        {
        }

        private class TransactionalImplement : ActionFilterAttribute
        {
            private readonly BaseDataContext _dataContext;

            public TransactionalImplement(BaseDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public override async Task OnActionExecutionAsync(
                ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                await using var transaction = await _dataContext.Database.BeginTransactionAsync();

                var result = await next();
                if (result.Exception == null) await transaction.CommitAsync();
                else await transaction.RollbackAsync();
            }
        }
    }
}