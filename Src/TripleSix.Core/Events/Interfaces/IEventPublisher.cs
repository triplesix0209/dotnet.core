using TripleSix.Core.Dto;

namespace TripleSix.Core.Events
{
    public interface IEventPublisher
    {
        void Raise<TEvent>(TEvent payload, IIdentity identity)
            where TEvent : IEvent;
    }
}