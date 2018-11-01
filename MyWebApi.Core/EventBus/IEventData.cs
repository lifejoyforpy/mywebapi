using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus
{



    /// <summary>
    /// 事件参数对象 Defines interface for all Event data classes.
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件id
        /// </summary>
        string EventId { get; set; }
        /// <summary>
        /// 事件源
        /// </summary>
        object EventSource { get; set; }

        DateTime EventTime { get; set; }
    }
}
