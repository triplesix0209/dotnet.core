#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Repositories
{
    public interface IRepository<TEntity> : IRepository
        where TEntity : class, IEntity
    {
        IQueryable<TEntity> Query { get; }

        IQueryable<TEntity> BuildQuery();

        Task<IQueryable<TEntity>> BuildQueryOfFilter(IIdentity identity, IFilterDto filter, Type filterType);

        Task<IQueryable<TEntity>> BuildQueryOfFilter(IFilterDto filter, Type filterType);

        Task<IQueryable<TEntity>> BuildQueryOfFilter<TFilterDto>(IIdentity identity, TFilterDto filter)
            where TFilterDto : IFilterDto;

        Task<IQueryable<TEntity>> BuildQueryOfFilter<TFilterDto>(TFilterDto filter)
            where TFilterDto : IFilterDto;

        void Create(IIdentity identity, params TEntity[] entities);

        void Create(params TEntity[] entities);

        void Create(IIdentity identity, IEnumerable<TEntity> entities);

        void Create(IEnumerable<TEntity> entities);

        void Update(IIdentity identity, params TEntity[] entities);

        void Update(params TEntity[] entities);

        void Update(IIdentity identity, IEnumerable<TEntity> entities);

        void Update(IEnumerable<TEntity> entities);

        void Delete(IIdentity identity, params TEntity[] entities);

        void Delete(params TEntity[] entities);

        void Delete(IIdentity identity, IEnumerable<TEntity> entities);

        void Delete(IEnumerable<TEntity> entities);
    }
}