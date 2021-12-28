using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Dto;
using CpTech.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CpTech.Core.Repositories
{
    public abstract class BaseRepository<TEntity> : BaseRepository,
        IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected BaseRepository(DbContext dataContext)
            : base(dataContext)
        {
        }

        public IQueryable<TEntity> Query => BuildQuery();

        public virtual IQueryable<TEntity> BuildQuery()
        {
            return DataContext.Set<TEntity>();
        }

        public virtual Task<IQueryable<TEntity>> BuildQuery(IIdentity identity, PagingFilterDto filter)
        {
            return Task.FromResult(BuildQuery());
        }

        public virtual Task<IQueryable<TEntity>> BuildQueryOfFilter(IIdentity identity, IFilterDto filter, Type filterType)
        {
            var repoType = GetType();
            var queryBuilderInterface = typeof(IQueryBuilder<,>).MakeGenericType(typeof(TEntity), filterType);
            if (!queryBuilderInterface.IsAssignableFrom(repoType))
                throw new Exception($"{repoType.Name} need implement IQueryBuilder<{typeof(TEntity).Name},{filterType.Name}> interface");

            var buildQueryMethod = repoType.GetMethods()
                .First(x => x.Name == nameof(IQueryBuilder<TEntity, IFilterDto>.BuildQuery) && x.GetParameters().Length >= 2 && x.GetParameters()[1].ParameterType == filterType);
            return (Task<IQueryable<TEntity>>)buildQueryMethod.Invoke(this, new object[] { identity, filter });
        }

        public Task<IQueryable<TEntity>> BuildQueryOfFilter(IFilterDto filter, Type filterType)
        {
            return BuildQueryOfFilter(null, filter, filterType);
        }

        public Task<IQueryable<TEntity>> BuildQueryOfFilter<TFilterDto>(IIdentity identity, TFilterDto filter)
            where TFilterDto : IFilterDto
        {
            return BuildQueryOfFilter(identity, filter, typeof(TFilterDto));
        }

        public Task<IQueryable<TEntity>> BuildQueryOfFilter<TFilterDto>(TFilterDto filter)
            where TFilterDto : IFilterDto
        {
            return BuildQueryOfFilter(null, filter, typeof(TFilterDto));
        }

        public virtual void Create(IIdentity identity, params TEntity[] entities)
        {
            DataContext.Set<TEntity>().AddRange(entities);
        }

        public void Create(params TEntity[] entities)
        {
            Create(null, entities);
        }

        public void Create(IIdentity identity, IEnumerable<TEntity> entities)
        {
            Create(identity, entities.ToArray());
        }

        public void Create(IEnumerable<TEntity> entities)
        {
            Create(null, entities.ToArray());
        }

        public virtual void Update(IIdentity identity, params TEntity[] entities)
        {
            DataContext.Set<TEntity>().UpdateRange(entities);
        }

        public void Update(params TEntity[] entities)
        {
            Update(null, entities);
        }

        public void Update(IIdentity identity, IEnumerable<TEntity> entities)
        {
            Update(identity, entities.ToArray());
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            Update(null, entities.ToArray());
        }

        public virtual void Delete(IIdentity identity, params TEntity[] entities)
        {
            DataContext.Set<TEntity>().RemoveRange(entities);
        }

        public void Delete(params TEntity[] entities)
        {
            Delete(null, entities);
        }

        public void Delete(IIdentity identity, IEnumerable<TEntity> entities)
        {
            Delete(identity, entities.ToArray());
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            Delete(null, entities.ToArray());
        }
    }
}