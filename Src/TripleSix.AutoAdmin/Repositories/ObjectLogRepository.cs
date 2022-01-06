using Microsoft.EntityFrameworkCore;
using TripleSix.AutoAdmin.Entities;
using TripleSix.Core.Repositories;

namespace TripleSix.AutoAdmin.Repositories
{
    public class ObjectLogRepository : ModelRepository<ObjectLogEntity>
    {
        public ObjectLogRepository(DbContext dataContext)
            : base(dataContext)
        {
        }
    }
}