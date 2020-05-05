using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.Core.EventBus
{
    /// <summary>
    /// 定义事件处理器公共接口
    /// </summary>
    public interface IEventHandler
    {
        Task Hanlder(IEventData @event);
    }


    public interface IEventHanlder<in TEvent> where TEvent:EventData
    {
        Task Hanlder(TEvent @event);
    }
}
