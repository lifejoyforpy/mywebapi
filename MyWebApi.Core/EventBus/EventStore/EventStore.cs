using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus.EventStore
{
    /// <summary>
    /// store eventHandler  
    /// </summary>
    public class EventStore
    {

        /// <summary>
        /// store handler
        /// </summary>
        public readonly Dictionary<Type, Type> _eventHandlers=new Dictionary<Type,Type>();
        /// <summary>
        /// add hanlders 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHanlder"></typeparam>
        public void Add<TEvent, TEventHanlder>() where TEventHanlder : IEventHanlder<TEvent> where TEvent : EventData
        {
            _eventHandlers.Add(typeof(TEvent), typeof(TEventHanlder));
        }  
        /// <summary>
        /// IF NOT ADD Handler
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHanlder"></typeparam>
        public void AddIfNot<TEvent, TEventHanlder>() where TEventHanlder : IEventHanlder<TEvent> where TEvent : EventData
        {         
            _eventHandlers.TryAdd(typeof(TEvent), typeof(TEventHanlder));
        }
        /// <summary>
        /// get handler by IServiceProvider
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object GetEventHanlder(Type eventType, IServiceProvider serviceProvider)
        {
            if (eventType == null || !_eventHandlers.TryGetValue(eventType, out var handlerType) && handlerType == null)
                return null;
           return  serviceProvider.GetService(handlerType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object GetEventHanlder(EventData eventData, IServiceProvider serviceProvider) 
        {
            return GetEventHanlder(eventData.GetType(), serviceProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object GetEventHanler<TEvent>(IServiceProvider serviceProvider) where TEvent:EventData
        {
            return GetEventHanlder(typeof(TEvent), serviceProvider);
        }

    }
}
