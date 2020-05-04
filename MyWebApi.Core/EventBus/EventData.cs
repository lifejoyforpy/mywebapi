using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    public class EventData : IEventData
    {
        public string EventId { get ; set ; }
        public object EventSource { get; set ; }
        public DateTimeOffset EventTime { get ; set ; }

        public EventData()
        {
            EventTime =DateTime.UtcNow ;
        }
    }
}
