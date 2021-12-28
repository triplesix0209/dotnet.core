using System.Threading.Tasks;
using CpTech.Core.Dto;
using CpTech.Core.Entities;

namespace CpTech.Core.Services
{
    public interface ILinkWithMapEntity<TMapEntity>
        where TMapEntity : class, IEntity
    {
        Task AddLink(IIdentity identity, TMapEntity mapEntity);

        Task RemoveLink(IIdentity identity, TMapEntity mapEntity);
    }
}
