using Sample.Data.DataContexts;
using Sample.Data.Entities;
using TripleSix.Core.Repositories;

namespace Sample.Data.Repositories
{
    public class PermissionRepository : BaseRepository<PermissionEntity>
    {
        public PermissionRepository(DataContext context)
            : base(context)
        {
        }
    }
}
