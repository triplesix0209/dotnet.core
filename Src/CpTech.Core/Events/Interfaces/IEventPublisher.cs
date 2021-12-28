using CpTech.Core.Dto;

namespace CpTech.Core.Events
{
    public interface IEventPublisher
    {
        void Raise<TEvent>(TEvent payload, IIdentity identity)
            where TEvent : IEvent;
    }
}