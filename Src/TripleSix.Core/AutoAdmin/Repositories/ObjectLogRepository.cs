using Microsoft.EntityFrameworkCore;
using TripleSix.Core.Repositories;

namespace TripleSix.Core.AutoAdmin
{
    public class ObjectLogRepository : BaseRepository<ObjectLogEntity>
    {
        public ObjectLogRepository(DbContext dataContext)
            : base(dataContext)
        {
        }
    }
}