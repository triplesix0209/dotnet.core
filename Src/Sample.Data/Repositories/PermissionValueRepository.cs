using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.Core.Repositories;

namespace Sample.Data.Repositories
{
    public class PermissionValueRepository : BaseRepository<PermissionValueEntity>
    {
        public PermissionValueRepository(DataContext context)
            : base(context)
        {
        }
    }
}
