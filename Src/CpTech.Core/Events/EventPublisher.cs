using System.Collections.Generic;
using System.Linq;
using CpTech.Core.Dto;

namespace CpTech.Core.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEnumerable<IEventListener> _listeners;

        public EventPublisher(IEnumerable<IEventListener> listeners)
        {
            _listeners = listeners;
        }

        public void Raise<TEvent>(TEvent payload, IIdentity identity)
            where TEvent : IEvent
        {
            var handlers = _listeners.OfType<IEventListener<TEvent>>();
            foreach (var handler in handlers)
            {
                handler.HandleEvent(payload, identity).Start();
            }
        }
    }
}