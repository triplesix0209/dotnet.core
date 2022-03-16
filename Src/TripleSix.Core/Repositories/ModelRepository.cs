using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Attributes;
using TripleSix.Core.AutoAdmin;
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
            var propertyInfo = filter.GetType().GetProperty(nameof(IModelFilterDto.SortColumn));
            var metadata = propertyInfo.GetCustomAttribute<SortColumnAttribute>();

            string entityName = null;
            if (metadata is not null && metadata.EntityName.IsNotNullOrWhiteSpace())
                entityName = metadata.EntityName;
            else if (propertyInfo.ReflectedType is not null && propertyInfo.ReflectedType.DeclaringType.IsAssignableTo<IAdminDto>())
                entityName = propertyInfo.ReflectedType.DeclaringType.GetEntityType().Name;

            var vaildColumns = new List<string>();
            if (entityName.IsNotNullOrWhiteSpace())
            {
                var entityType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetExportedTypes())
                    .Where(x => x.IsAssignableTo<IEntity>())
                    .Where(x => x.Name == entityName)
                    .FirstOrDefault();

                if (entityType is not null)
                {
                    vaildColumns.AddRange(entityType.GetProperties()
                        .Where(x => !x.PropertyType.IsAssignableTo<IEntity>())
                        .Where(x => !x.PropertyType.IsSubclassOfRawGeneric(typeof(IList<>)))
                        .Select(x => x.Name.ToCamelCase()));
                }
            }

            var externalColumns = metadata?.ExternalColumns
                .Select(x => x.ToCamelCase());
            if (externalColumns.IsNotNullOrEmpty())
            {
                foreach (var columnName in externalColumns)
                {
                    if (vaildColumns.Any(x => x == columnName)) continue;
                    vaildColumns.Add(columnName);
                }
            }

            var invaildColumns = filter.SortColumn?
                .Where(x => !vaildColumns.Contains(x.Name));
            if (invaildColumns.IsNotNullOrEmpty())
            {
                throw new BaseException(
                    BaseExceptions.SortColumnInvalid,
                    args: string.Join(",", invaildColumns.Select(x => x.Name)));
            }

            return (await BuildQueryAuto(identity, filter))
                .OrderBySortColumn(filter.SortColumn);
        }
    }
}
