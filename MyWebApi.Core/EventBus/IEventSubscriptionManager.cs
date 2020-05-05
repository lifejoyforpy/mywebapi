using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.Core.EventBus
{
    public interface IEventSubscriptionManager
    {

        Task Subscribe<TEvent, TEventHanler>() where TEventHanler : IEventHanlder<TEvent> where TEvent : EventData;
    }
    /// <summary>
    /// subscribe 
    /// </summary>

    public class EventSubscriptionManager : IEventSubscriptionManager
    {

        private readonly EventStore.EventStore _eventStore;
        public EventSubscriptionManager(EventStore.EventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public Task Subscribe<TEvent, TEventHanler>()
            where TEventHanler : IEventHanlder<TEvent>
            where TEvent : EventData    
        {
            _eventStore.Add<TEvent,TEventHanler>();
            return Task.CompletedTask;
        }
    }
}
