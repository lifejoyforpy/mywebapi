using System;
using System.Threading.Tasks;

namespace MyWebApi.Core.EventBus
{
    /// <summary>
    /// publish event
    /// </summary>
    public interface IEventPublish
    {

        Task Publish<TEvent>(string queueName ,TEvent @event) where TEvent:EventData;

        Task Publish(string queueName, EventData @event);
    }

    public class EventPublish : IEventPublish
    {
        private readonly EventQueue.EventQueue _eventQueue;

        public EventPublish(EventQueue.EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }
        /// <summary>
        ///  publish a event into a queue
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task Publish<TEvent>(string queueName, TEvent @event) where TEvent : EventData
        {
            _eventQueue.EnQueue(queueName, @event);
            return Task.CompletedTask;
        }
        /// <summary>
        /// publish a event into a queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task Publish(string queueName, EventData @event)
        {
            _eventQueue.EnQueue(queueName, @event);
            return Task.CompletedTask;
        }
    }
}
