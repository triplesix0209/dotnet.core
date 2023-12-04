#pragma warning disable SA1649 // File name should match first type name

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace TripleSix.CoreOld.Repositories
{
    public interface IRepository
    {
        Task SaveChanges();

        Task<IDbContextTransaction> BeginTransaction();
    }
}