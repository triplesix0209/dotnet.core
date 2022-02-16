using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Repositories
{
    public abstract class ModelRepository<TEntity> : BaseRepository<TEntity>,
        IModelRepository<TEntity>
        where TEntity : class, IModelEntity
    {
        protected ModelRepository(DbContext dataContext)
            : base(dataContext)
        {
        }

        public override void Create(IIdentity identity, params TEntity[] entities)
        {
            if (identity != null)
            {
                foreach (var entity in entities)
                {
                    entity.CreatorId = identity.UserId;
                    entity.UpdaterId = identity.UserId;
                }
            }

            var codes = entities.Where(e => e.Code != null).Select(e => e.Code);
            if (codes.Any())
            {
                var duplicatedCodes = Query
                    .Where(x => x.Code != null)
                    .Where(x => codes.Contains(x.Code))
                    .Select(x => $"'{x.Code}'")
                    .ToArray();
                if (duplicatedCodes.Any())
                {
                    throw new BaseException(
                        BaseExceptions.CodeDuplicated,
                        args: new[] { typeof(TEntity).GetDisplayName(), string.Join(", ", duplicatedCodes) });
                }
            }

            base.Create(identity, entities);
        }

        public override void Update(IIdentity identity, params TEntity[] entities)
        {
            if (identity != null)
            {
                foreach (var entity in entities)
                {
                    entity.UpdaterId = identity.UserId;
                }
            }

            var codes = entities.Where(e => e.Code != null).Select(e => e.Code);
            if (codes.Any())
            {
                var duplicatedCodes = Query
                    .Where(x => x.Code != null)
                    .Where(x => !entities.Select(e => e.Id).Contains(x.Id))
                    .Where(x => codes.Contains(x.Code))
                    .Select(x => $"'{x.Code}'")
                    .ToArray();
                if (duplicatedCodes.Any())
                {
                    throw new BaseException(
                        BaseExceptions.CodeDuplicated,
                        args: new[] { typeof(TEntity).GetDisplayName(), string.Join(", ", duplicatedCodes) });
                }
            }

            base.Update(identity, entities);
        }

        public virtual void SetAsDeleted(IIdentity identity, params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

            Update(identity, entities);
        }

        public void SetAsDeleted(params TEntity[] entities)
        {
            SetAsDeleted(null, entities);
        }

        public void SetAsDeleted(IIdentity identity, IEnumerable<TEntity> entities)
        {
            SetAsDeleted(identity, entities.ToArray());
        }

        public void SetAsDeleted(IEnumerable<TEntity> entities)
        {
            SetAsDeleted(null, entities.ToArray());
        }

        public virtual void Restore(IIdentity identity, params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = false;
            }

            Update(identity, entities);
        }

        public void Restore(params TEntity[] entities)
        {
            Restore(null, entities);
        }

        public void Restore(IIdentity identity, IEnumerable<TEntity> entities)
        {
            Restore(identity, entities.ToArray());
        }

        public void Restore(IEnumerable<TEntity> entities)
        {
            Restore(null, entities.ToArray());
        }

        public virtual async Task<IQueryable<TEntity>> BuildQuery(IIdentity identity, ModelFilterDto filter)
        {
            var query = await BuildQueryAuto(identity, filter);

            break;
            //if (filter.SortColumns.IsNotNullOrWhiteSpace())
            //{
            //    var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //    IOrderedQueryable<TEntity> orderedQuery = null;
            //    var sorts = filter.SortColumns.Split(",")
            //        .Select(x =>
            //        {
            //            var items = x.Split("=");
            //            for (var i = 0; i < items.Length; i++)
            //                items[i] = items[i].Trim().ToLower();
            //            return items;
            //        });
            //    foreach (var sort in sorts)
            //    {
            //        var isAscending = !(sort.Length > 1 && sort[1] == "desc");
            //        var propertyName = properties.FirstOrDefault(x => x.Name.ToLower() == sort[0])?.Name;
            //        if (propertyName == null)
            //            throw new Exception($"column \"{sort[0]}\" not found in {typeof(TEntity).Name}");
            //        if (orderedQuery == null)
            //        {
            //            orderedQuery = isAscending
            //                ? query.OrderBy(e => EF.Property<object>(e, propertyName))
            //                : query.OrderByDescending(e => EF.Property<object>(e, propertyName));
            //        }
            //        else
            //        {
            //            orderedQuery = isAscending
            //                ? orderedQuery.ThenBy(e => EF.Property<object>(e, propertyName))
            //                : orderedQuery.ThenByDescending(e => EF.Property<object>(e, propertyName));
            //        }
            //    }
            //    query = orderedQuery;
            //}

            return query;
        }
    }
}