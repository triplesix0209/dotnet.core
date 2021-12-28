using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace CpTech.Core.Repositories
{
    public abstract class BaseRepository
        : IRepository
    {
        protected BaseRepository(DbContext dataContext)
        {
            DataContext = dataContext;
        }

        public IConfiguration Configuration { get; set; }

        public IMapper Mapper { get; set; }

        protected virtual DbContext DataContext { get; }

        public virtual async Task SaveChanges()
        {
            await DataContext.SaveChangesAsync();
        }

        public virtual Task<IDbContextTransaction> BeginTransaction()
        {
            return DataContext.Database.BeginTransactionAsync();
        }
    }
}