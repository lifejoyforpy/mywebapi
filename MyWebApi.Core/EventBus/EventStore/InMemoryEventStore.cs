using MyWebApi.Core.EventBus.Handlers.Internals;
using MyWebApi.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebApi.Core.EventBus.EventStore
{
    public class InMemoryEventStore : IEventStore
    {

        private static readonly object synclock = new object();
        //存储数据源和处理器
        private readonly ConcurrentDictionary<Type, List<Type>> _eventAndHandlerMapping;
        public InMemoryEventStore()
        {
            _eventAndHandlerMapping = new ConcurrentDictionary<Type, List<Type>>();
        }
        public bool IsEmpty => !_eventAndHandlerMapping.Keys.Any();

        public void AddActionRegister<T>(Action<T> action) where T : IEventData
        {
            var actionHandler= new ActionEventHandler<T>(action);
            this.AddRegister(typeof(T), actionHandler.GetType());
        }

        public void AddRegister<T, TH>()
            where T : IEventData
            where TH : IEventHandler
        {
            this.AddRegister(typeof(T), typeof(TH));
        }

        public void AddRegister(Type eventData, Type eventHandler)
        {
            lock (synclock)
            {
                if (this.HasRegisterForEvent(eventData))
                {
                    var handlerTypes = _eventAndHandlerMapping[eventData];
                    handlerTypes.AddIfNot(eventHandler);
                    _eventAndHandlerMapping[eventData] = handlerTypes;
                }
                else
                {
                    _eventAndHandlerMapping.TryAdd(eventData, new List<Type> {
                           eventHandler });
                }

            }
        }

        public void Clear()=> _eventAndHandlerMapping.Clear();

        public Type GetEventTypeByName(string eventName)
        {
            return _eventAndHandlerMapping.Keys.FirstOrDefault(eh => eh.Name == eventName);
        }

        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData
        {
            return GetHandlersForEvent(typeof(T));
        }

        public IEnumerable<Type> GetHandlersForEvent(Type eventData)
        {
            if (HasRegisterForEvent(eventData))
            {
                return _eventAndHandlerMapping[eventData];
            }
            return new List<Type>();
        }

        public bool HasRegisterForEvent<T>() where T : IEventData
        {
            return _eventAndHandlerMapping.ContainsKey(typeof(T));
        }

        public bool HasRegisterForEvent(Type eventData)
        {
            return _eventAndHandlerMapping.ContainsKey(eventData);
        }

        public void RemoveActionRegister<T>(Action<T> action) where T : IEventData
        {
            var actionHandler= new ActionEventHandler<T>(action);
            //查找
            var handlertype=FindRegisterToRemove(typeof(T), actionHandler.GetType());
            RemoveRegister(typeof(T), handlertype);
        }

        public void RemoveRegister<T, TH>()
            where T : IEventData
            where TH : IEventHandler
        {
            this.RemoveRegister(typeof(T), typeof(TH));
        }

        public void RemoveRegister(Type eventData, Type eventHandler)
        {
            if (eventHandler != null)
            {
                lock (synclock)
                {
                    _eventAndHandlerMapping[eventData].Remove(eventHandler);

                }
            }
        }

        private Type FindRegisterToRemove(Type eventData, Type eventHandler)
        {
            if (!HasRegisterForEvent(eventData))
            {
                return null;
            }

            return _eventAndHandlerMapping[eventData].FirstOrDefault(eh => eh == eventHandler);
        }

    }
}
