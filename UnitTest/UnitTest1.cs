using MyWebApi.Core.EventBus;
using MyWebApi.Core.EventBus.Handlers;
using System;
using System.Reflection;
using Xunit;

namespace UnitTest
{
   
    public class TestHandler : IEventHandler<EventData>
    {
        public void HandlerEvent(EventData eventData)
        {
            Console.WriteLine("EvenetBus Test");
           
        }
    }
    public class UnitTest1
    {
        private IEventBus eventBus;
        public UnitTest1()
        {
            eventBus = EventBus.Default;
            eventBus.RegisterAllEventHandlerFromAssembly(Assembly.GetExecutingAssembly());         

        }
        [Fact]
        public void Test1()
        {
            eventBus.Trigger<EventData>(new EventData());
           // Console.ReadLine();
        }
    }
}
