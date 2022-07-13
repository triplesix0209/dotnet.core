﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.Persistences;

namespace TripleSix.Core.WebApi
{
    public class Transactional : TypeFilterAttribute
    {
        public Transactional()
            : base(typeof(TransactionalImplement))
        {
        }

        private class TransactionalImplement : ActionFilterAttribute
        {
            private readonly BaseDbContext _dbContext;

            public TransactionalImplement(BaseDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public override async Task OnActionExecutionAsync(
                ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                var result = await next();
                if (result.Exception == null) await transaction.CommitAsync();
                else await transaction.RollbackAsync();
            }
        }
    }
}