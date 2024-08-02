﻿using Microsoft.AspNetCore.Http;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO query.
    /// </summary>
    /// <typeparam name="TEntity">Loại entity.</typeparam>
    public abstract class BaseEntityQueryDto<TEntity> : BaseDto,
        IEntityQueryableDto<TEntity>
        where TEntity : IEntity
    {
        /// <inheritdoc/>
        public abstract IQueryable<TEntity> ToQueryable(IQueryable<TEntity> query, IHttpContextAccessor? httpContextAccessor);
    }
}
