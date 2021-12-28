using System.Threading.Tasks;
using CpTech.Core.Dto;

namespace CpTech.Core.Events
{
    public interface IEventListener<in TEvent>
        : IEventListener
        where TEvent : IEvent
    {
        Task HandleEvent(TEvent payload, IIdentity identity);
    }
}