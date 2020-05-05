
using MyWebApi.Core.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class TestEvent : EventData
    {
       
    }
    /// <summary>
    /// 
    /// </summary>
    public class TestEventHandler : EventHanlderBase<TestEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task Hanlder(TestEvent @event)
        {
            Console.WriteLine("asbb");
            return Task.CompletedTask;
        }
    }
}
