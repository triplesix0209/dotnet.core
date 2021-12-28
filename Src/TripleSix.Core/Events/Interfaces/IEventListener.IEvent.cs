using System.Threading.Tasks;
using TripleSix.Core.Dto;

namespace TripleSix.Core.Events
{
    public interface IEventListener<in TEvent>
        : IEventListener
        where TEvent : IEvent
    {
        Task HandleEvent(TEvent payload, IIdentity identity);
    }
}