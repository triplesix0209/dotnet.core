using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;

namespace TripleSix.Core.Helpers
{
    public static class LinqHelper
    {
        public static TResult First<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            TResult item;
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType))
            {
                item = query.FirstOrDefault() as TResult;
            }
            else
            {
                item = query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .FirstOrDefault();
            }

            if (item == null) throw new BaseException(BaseExceptions.ObjectNotFound, args: typeof(TResult).GetDisplayName());
            return item;
        }

        public static async Task<TResult> FirstAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            TResult item;
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType))
            {
                item = await query.FirstOrDefaultAsync() as TResult;
            }
            else
            {
                item = await query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            }

            if (item == null)
                throw new BaseException(BaseExceptions.ObjectNotFound, args: typeof(TResult).GetDisplayName());
            return item;
        }

        public static TResult FirstOrDefault<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return query.FirstOrDefault() as TResult;

            return query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        public static async Task<TResult> FirstOrDefaultAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return await query.FirstOrDefaultAsync() as TResult;

            return await query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public static TResult[] ToArray<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return query.ToArray().Cast<TResult>().ToArray();

            return query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToArray();
        }

        public static async Task<TResult[]> ToArrayAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return (await query.ToArrayAsync()).Cast<TResult>().ToArray();

            return await query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public static List<TResult> ToList<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return query.ToArray().Cast<TResult>().ToList();

            return query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToList();
        }

        public static async Task<List<TResult>> ToListAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return (await query.ToArrayAsync()).Cast<TResult>().ToList();

            return await query
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public static IPaging<TResult> ToPaging<TResult>(this IQueryable<IEntity> query, IMapper mapper, int page, int size)
            where TResult : class
        {
            var result = new Paging<TResult>
            {
                Total = query.LongCount(),
                Page = page,
                Size = size,
                Items = Array.Empty<TResult>(),
            };
            if (result.Total == 0) return result;

            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType))
            {
                result.Items = query.Skip((page - 1) * size).Take(size).ToArray().Cast<TResult>().ToArray();
            }
            else
            {
                result.Items = query.ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToArray();
            }

            return result;
        }

        public static async Task<IPaging<TResult>> ToPagingAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper, int page, int size)
            where TResult : class
        {
            var result = new Paging<TResult>
            {
                Total = query.LongCount(),
                Page = page,
                Size = size,
                Items = Array.Empty<TResult>(),
            };
            if (result.Total == 0) return result;

            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType))
            {
                result.Items = (await query.Skip((page - 1) * size).Take(size).ToArrayAsync()).Cast<TResult>().ToArray();
            }
            else
            {
                result.Items = await query.ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToArrayAsync();
            }

            return result;
        }

        public static IEnumerable<TEntity> WhereNotDeleted<TEntity>(this IEnumerable<TEntity> query)
            where TEntity : IModelEntity
        {
            return query.Where(x => !x.IsDeleted);
        }

        public static IQueryable<TEntity> WhereNotDeleted<TEntity>(this IQueryable<TEntity> query)
            where TEntity : IModelEntity
        {
            return query.Where(x => !x.IsDeleted);
        }

        public static IQueryable<TEntity> WhereOrs<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : IEntity
        {
            var expr = PredicateBuilder.New<TEntity>();
            foreach (var predicate in predicates)
                expr = expr.Or(predicate);
            return query.Where(expr);
        }
    }
}
