﻿using System.Linq;
using System.Threading.Tasks;
using Sample.Common.Dto;
using Sample.Data.DataContexts;
using Sample.Data.Entities;

namespace Sample.Data.Repositories
{
    public class PermissionGroupRepository : ModelRepository<PermissionGroupEntity>,
        IQueryBuilder<PermissionGroupEntity, PermissionGroupAdminDto.Filter>
    {
        public PermissionGroupRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IQueryable<PermissionGroupEntity>> BuildQuery(IIdentity identity, PermissionGroupAdminDto.Filter filter)
        {
            var query = await BuildQuery(identity, filter as ModelFilterDto);

            if (filter.Search.IsNotNullOrWhiteSpace())
            {
                query = query.WhereOrs(
                x => EF.Functions.Like(x.Code, $"%{filter.Search}%"),
                x => EF.Functions.Like(x.Name, $"%{filter.Search}%"));
            }

            return query;
        }
    }
}
