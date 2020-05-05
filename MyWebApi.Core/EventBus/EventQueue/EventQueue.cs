using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus.EventQueue
{
    /// <summary>
    /// 按照队列 执行
    /// </summary>
    public class EventQueue
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<EventData>> _eventQueues = new ConcurrentDictionary<string, ConcurrentQueue<EventData>>();

        public  ICollection<string> QueueKeys=> _eventQueues.Keys ;

        public void EnQueue<TEvent>(string queueName, TEvent @event) where TEvent : EventData
        {
            var queue = _eventQueues.GetOrAdd(queueName, q => new ConcurrentQueue<EventData>());
            queue.Enqueue(@event);
        }
        /// <summary>
        /// del a event from a queue
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="event"></param>

        public bool TryDeQueue<TEvent>(string queueName, out EventData @event)
        {
            var queue = _eventQueues.GetOrAdd(queueName, q => new ConcurrentQueue<EventData>());
            return   queue.TryDequeue(out @event);
        }
        /// <summary>
        /// del queue
        /// </summary>
        /// <param name="queueName"></param>
        public bool  TryRemoveQueue(string queueName)
        {
           return  _eventQueues.TryRemove(queueName, out var _);
        }
        /// <summary>
        ///  if contains a queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public bool ContainsQueue(string queueName) => _eventQueues.ContainsKey(queueName);

        /// <summary>
        /// get a queue index 
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public ConcurrentQueue<EventData>  this[string queueName] => _eventQueues[queueName];
    }
}
