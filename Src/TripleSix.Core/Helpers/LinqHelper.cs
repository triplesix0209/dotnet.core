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

        public static IQueryable<TEntity> AppendFilterParameter<TEntity>(this IQueryable<TEntity> query, string fieldName, IFilterParameter filter)
            where TEntity : IEntity
        {
            if (filter is null) return query;
            var filterType = filter.GetType();

            if (filter is FilterParameterString)
            {
                return query.AppendFilterParameter(fieldName, filter as FilterParameterString);
            }
            else if (filter is FilterParameterDatetime)
            {
                return query.AppendFilterParameter(fieldName, filter as FilterParameterDatetime);
            }
            else if (filterType.IsSubclassOfRawGeneric(typeof(FilterParameterNumber<>)))
            {
                var method = typeof(LinqHelper).GetMethods()
                .Where(x => x.IsStatic)
                .Where(x => x.Name == nameof(AppendFilterParameter))
                .Where(x => x.GetGenericArguments().Length == 2)
                .Where(x => x.GetParameters()[2].ParameterType.IsSubclassOfRawGeneric(typeof(FilterParameterNumber<>)))
                .FirstOrDefault();

                return method
                    .MakeGenericMethod(typeof(TEntity), filter.GetType().GetGenericArguments()[0])
                    .Invoke(null, new object[] { query, fieldName, filter }) as IQueryable<TEntity>;
            }
            else
            {
                var method = typeof(LinqHelper).GetMethods()
                .Where(x => x.IsStatic)
                .Where(x => x.Name == nameof(AppendFilterParameter))
                .Where(x => x.GetGenericArguments().Length == 2)
                .Where(x => x.GetParameters()[2].ParameterType.IsSubclassOfRawGeneric(typeof(FilterParameter<>)))
                .FirstOrDefault();

                return method
                    .MakeGenericMethod(typeof(TEntity), filter.GetType().GetGenericArguments()[0])
                    .Invoke(null, new object[] { query, fieldName, filter }) as IQueryable<TEntity>;
            }
        }

        public static IQueryable<TEntity> AppendFilterParameter<TEntity, TType>(this IQueryable<TEntity> query, string fieldName, FilterParameter<TType> filter)
            where TEntity : IEntity
        {
            if (filter is null) return query;

            switch (filter.Operator)
            {
                case FilterParameterOperators.Is:
                    query = query.Where(x => EF.Property<TType>(x, fieldName).Equals(filter.Value[0]));
                    break;
                case FilterParameterOperators.In:
                    query = query.Where(x => filter.Value.Contains(EF.Property<TType>(x, fieldName)));
                    break;
                case FilterParameterOperators.IsNull:
                    query = query.Where(x => EF.Property<TType>(x, fieldName) == null);
                    break;
                case FilterParameterOperators.NotIs:
                    query = query.Where(x => !EF.Property<TType>(x, fieldName).Equals(filter.Value[0]));
                    break;
                case FilterParameterOperators.NotIn:
                    query = query.Where(x => !filter.Value.Contains(EF.Property<TType>(x, fieldName)));
                    break;
                case FilterParameterOperators.NotNull:
                    query = query.Where(x => EF.Property<TType>(x, fieldName) != null);
                    break;
            }

            return query;
        }

        public static IQueryable<TEntity> AppendFilterParameter<TEntity, TType>(this IQueryable<TEntity> query, string fieldName, FilterParameterNumber<TType> filter)
            where TEntity : IEntity
            where TType : IComparable
        {
            if (filter is null) return query;
            var pe = Expression.Parameter(typeof(TEntity));
            Expression expr;

            switch (filter.Operator)
            {
                case FilterParameterNumberOperators.Equal:
                    query = query.Where(x => EF.Property<TType>(x, fieldName).Equals(filter.Value[0]));
                    break;
                case FilterParameterNumberOperators.Less:
                    expr = Expression.LessThan(
                        Expression.Convert(Expression.Property(pe, fieldName), typeof(int)),
                        Expression.Constant(filter.Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;
                case FilterParameterNumberOperators.LessOrEqual:
                    expr = Expression.LessThanOrEqual(
                        Expression.Convert(Expression.Property(pe, fieldName), typeof(int)),
                        Expression.Constant(filter.Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;
                case FilterParameterNumberOperators.Greater:
                    expr = Expression.GreaterThan(
                        Expression.Convert(Expression.Property(pe, fieldName), typeof(int)),
                        Expression.Constant(filter.Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;
                case FilterParameterNumberOperators.GreaterOrEqual:
                    expr = Expression.GreaterThanOrEqual(
                        Expression.Convert(Expression.Property(pe, fieldName), typeof(int)),
                        Expression.Constant(filter.Value[0]));
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(expr, pe));
                    break;
                case FilterParameterNumberOperators.In:
                    query = query.Where(x => filter.Value.Contains(EF.Property<TType>(x, fieldName)));
                    break;
                case FilterParameterNumberOperators.IsNull:
                    query = query.Where(x => EF.Property<TType>(x, fieldName) == null);
                    break;
                case FilterParameterNumberOperators.NotEqual:
                    query = query.Where(x => !EF.Property<TType>(x, fieldName).Equals(filter.Value[0]));
                    break;
                case FilterParameterNumberOperators.NotIn:
                    query = query.Where(x => !filter.Value.Contains(EF.Property<TType>(x, fieldName)));
                    break;
                case FilterParameterNumberOperators.NotNull:
                    query = query.Where(x => EF.Property<TType>(x, fieldName) != null);
                    break;
            }

            return query;
        }

        public static IQueryable<TEntity> AppendFilterParameter<TEntity>(this IQueryable<TEntity> query, string fieldName, FilterParameterString filter)
            where TEntity : IEntity
        {
            if (filter is null) return query;

            switch (filter.Operator)
            {
                case FilterParameterStringOperators.Equal:
                    query = query.Where(x => EF.Property<string>(x, fieldName) == filter.Value[0]);
                    break;
                case FilterParameterStringOperators.Contain:
                    query = query.Where(x => EF.Functions.Like(EF.Property<string>(x, fieldName), $"%{filter.Value[0]}%"));
                    break;
                case FilterParameterStringOperators.StartWith:
                    query = query.Where(x => EF.Functions.Like(EF.Property<string>(x, fieldName), $"{filter.Value[0]}%"));
                    break;
                case FilterParameterStringOperators.EndWith:
                    query = query.Where(x => EF.Functions.Like(EF.Property<string>(x, fieldName), $"%{filter.Value[0]}"));
                    break;
                case FilterParameterStringOperators.In:
                    query = query.Where(x => filter.Value.Contains(EF.Property<string>(x, fieldName)));
                    break;
                case FilterParameterStringOperators.IsNull:
                    query = query.Where(x => EF.Property<string>(x, fieldName) == null);
                    break;
                case FilterParameterStringOperators.NotEqual:
                    query = query.Where(x => !EF.Property<string>(x, fieldName).Equals(filter.Value[0]));
                    break;
                case FilterParameterStringOperators.NotContain:
                    query = query.Where(x => !EF.Functions.Like(EF.Property<string>(x, fieldName), $"%{filter.Value[0]}%"));
                    break;
                case FilterParameterStringOperators.NotStartWith:
                    query = query.Where(x => !EF.Functions.Like(EF.Property<string>(x, fieldName), $"{filter.Value[0]}%"));
                    break;
                case FilterParameterStringOperators.NotEndWith:
                    query = query.Where(x => !EF.Functions.Like(EF.Property<string>(x, fieldName), $"%{filter.Value[0]}"));
                    break;
                case FilterParameterStringOperators.NotIn:
                    query = query.Where(x => !filter.Value.Contains(EF.Property<string>(x, fieldName)));
                    break;
                case FilterParameterStringOperators.NotNull:
                    query = query.Where(x => EF.Property<string>(x, fieldName) != null);
                    break;
            }

            return query;
        }

        public static IQueryable<TEntity> AppendFilterParameter<TEntity>(this IQueryable<TEntity> query, string fieldName, FilterParameterDatetime filter)
                where TEntity : IEntity
        {
            if (filter is null) return query;

            switch (filter.Operator)
            {
                case FilterParameterDatetimeOperators.Is:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) == filter.Value[0]);
                    break;
                case FilterParameterDatetimeOperators.Begin:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) >= filter.Value[0]);
                    break;
                case FilterParameterDatetimeOperators.End:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) <= filter.Value[0]);
                    break;
                case FilterParameterDatetimeOperators.Between:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) >= filter.Value[0]
                        && EF.Property<DateTime>(x, fieldName) <= filter.Value[1]);
                    break;
                case FilterParameterDatetimeOperators.IsNull:
                    query = query.Where(x => EF.Property<string>(x, fieldName) == null);
                    break;
                case FilterParameterDatetimeOperators.NotIs:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) != filter.Value[0]);
                    break;
                case FilterParameterDatetimeOperators.NotBegin:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) < filter.Value[0]);
                    break;
                case FilterParameterDatetimeOperators.NotEnd:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) > filter.Value[0]);
                    break;
                case FilterParameterDatetimeOperators.NotBetween:
                    query = query.Where(x => EF.Property<DateTime>(x, fieldName) < filter.Value[0]
                        || EF.Property<DateTime>(x, fieldName) > filter.Value[1]);
                    break;
                case FilterParameterDatetimeOperators.NotNull:
                    query = query.Where(x => EF.Property<string>(x, fieldName) != null);
                    break;
            }

            return query;
        }
    }
}
