using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.Core.Repositories;

namespace Sample.Data.Repositories
{
    public class PermissionGroupRepository : ModelRepository<PermissionGroupEntity>
    {
        public PermissionGroupRepository(DataContext context)
            : base(context)
        {
        }
    }
}
