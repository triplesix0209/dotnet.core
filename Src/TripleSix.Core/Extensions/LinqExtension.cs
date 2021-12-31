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
using TripleSix.Core.Mappers;

namespace TripleSix.Core.Extensions
{
    public static class LinqExtension
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
                var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
                if (isFullMapper)
                {
                    item = mapper.MapData(
                        query.FirstOrDefault(),
                        query.ElementType,
                        resultType) as TResult;
                }
                else
                {
                    item = query
                        .ProjectTo<TResult>(mapper.ConfigurationProvider)
                        .FirstOrDefault();
                }
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
                var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
                if (isFullMapper)
                {
                    item = mapper.MapData(
                        await query.FirstOrDefaultAsync(),
                        query.ElementType,
                        resultType) as TResult;
                }
                else
                {
                    item = await query
                        .ProjectTo<TResult>(mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                }
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

            var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
            if (isFullMapper)
            {
                return mapper.MapData(
                    query.FirstOrDefault(),
                    query.ElementType,
                    resultType) as TResult;
            }
            else
            {
                return query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .FirstOrDefault();
            }
        }

        public static async Task<TResult> FirstOrDefaultAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return await query.FirstOrDefaultAsync() as TResult;

            var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
            if (isFullMapper)
            {
                return mapper.MapData(
                    await query.FirstOrDefaultAsync(),
                    query.ElementType,
                    resultType) as TResult;
            }
            else
            {
                return await query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            }
        }

        public static TResult[] ToArray<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return query.ToArray().Cast<TResult>().ToArray();

            var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
            if (isFullMapper)
            {
                return mapper.MapData(
                    query.ToArray(),
                    query.ElementType.MakeArrayType(),
                    resultType.MakeArrayType()) as TResult[];
            }
            else
            {
                return query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .ToArray();
            }
        }

        public static async Task<TResult[]> ToArrayAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return (await query.ToArrayAsync()).Cast<TResult>().ToArray();

            var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
            if (isFullMapper)
            {
                return mapper.MapData(
                    await query.ToArrayAsync(),
                    query.ElementType.MakeArrayType(),
                    resultType.MakeArrayType()) as TResult[];
            }
            else
            {
                return await query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .ToArrayAsync();
            }
        }

        public static List<TResult> ToList<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return query.ToArray().Cast<TResult>().ToList();

            var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
            if (isFullMapper)
            {
                return mapper.MapData(
                    query.ToList(),
                    query.ElementType.MakeGenericType(typeof(List<>)),
                    resultType.MakeGenericType(typeof(List<>))) as List<TResult>;
            }
            else
            {
                return query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .ToList();
            }
        }

        public static async Task<List<TResult>> ToListAsync<TResult>(this IQueryable<IEntity> query, IMapper mapper)
            where TResult : class
        {
            var resultType = typeof(TResult);
            if (typeof(IEntity).IsAssignableFrom(resultType)) return (await query.ToArrayAsync()).Cast<TResult>().ToList();

            var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
            if (isFullMapper)
            {
                return mapper.MapData(
                    await query.ToListAsync(),
                    query.ElementType.MakeGenericType(typeof(List<>)),
                    resultType.MakeGenericType(typeof(List<>))) as List<TResult>;
            }
            else
            {
                return await query
                    .ProjectTo<TResult>(mapper.ConfigurationProvider)
                    .ToListAsync();
            }
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
                var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
                if (isFullMapper)
                {
                    result.Items = mapper.MapData(
                        query.Skip((page - 1) * size).Take(size).ToArray(),
                        query.ElementType.MakeArrayType(),
                        resultType.MakeArrayType()) as TResult[];
                }
                else
                {
                    result.Items = query.ProjectTo<TResult>(mapper.ConfigurationProvider)
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToArray();
                }
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
                var isFullMapper = CheckFullMapper(mapper, query.ElementType, resultType);
                if (isFullMapper)
                {
                    result.Items = mapper.MapData(
                        await query.Skip((page - 1) * size).Take(size).ToArrayAsync(),
                        query.ElementType.MakeArrayType(),
                        resultType.MakeArrayType()) as TResult[];
                }
                else
                {
                    result.Items = await query.ProjectTo<TResult>(mapper.ConfigurationProvider)
                        .Skip((page - 1) * size)
                        .Take(size)
                        .ToArrayAsync();
                }
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

        public static IQueryable<TEntity> WhereAppendIdOr<TEntity>(this IQueryable<TEntity> query, IModelFilterDto filter, Expression<Func<TEntity, bool>> predicate)
            where TEntity : IModelEntity
        {
            var expr = PredicateBuilder.New(predicate);

            if (filter.AppendIds.IsNotNullOrWhiteSpace())
            {
                var appendIds = filter.AppendIds.Split(",").Select(x => Guid.Parse(x));
                expr = expr.Or(x => appendIds.Contains(x.Id));
            }

            return query.Where(expr);
        }

        public static IQueryable<TEntity> WhereOrs<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : IModelEntity
        {
            var expr = PredicateBuilder.New<TEntity>();
            foreach (var predicate in predicates)
                expr = expr.Or(predicate);
            return query.Where(expr);
        }

        private static bool CheckFullMapper(IMapper mapper, Type sourceType, Type destinationType)
        {
            var typeMap = mapper.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);
            if (typeMap == null) return false;

            var isFullMapper = typeMap.BeforeMapActions.Any() || typeMap.AfterMapActions.Any();
            if (isFullMapper && !typeof(IFullMapperDto).IsAssignableFrom(destinationType))
                throw new Exception($"{destinationType.Name} need implement {nameof(IFullMapperDto)} for using BeforeMap and AfterMap.");

            return isFullMapper;
        }
    }
}