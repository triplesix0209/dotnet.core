using System.Threading.Tasks;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;

namespace TripleSix.Core.Services
{
    public interface ILinkWithMapEntity<TMapEntity>
        where TMapEntity : class, IEntity
    {
        Task AddLink(IIdentity identity, TMapEntity mapEntity);

        Task RemoveLink(IIdentity identity, TMapEntity mapEntity);
    }
}
