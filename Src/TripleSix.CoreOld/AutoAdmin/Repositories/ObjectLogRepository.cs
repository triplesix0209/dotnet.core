using Microsoft.EntityFrameworkCore;
using TripleSix.CoreOld.Repositories;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class ObjectLogRepository : BaseRepository<ObjectLogEntity>
    {
        public ObjectLogRepository(DbContext dataContext)
            : base(dataContext)
        {
        }
    }
}