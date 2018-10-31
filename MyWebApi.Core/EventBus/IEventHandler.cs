using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    /// <summary>
    /// 定义事件处理器公共接口
    /// </summary>
    public interface IEventHandler
    {


    }

    /// <summary>
    /// 泛型事件处理器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public interface IEventHandler<TEventDaTa>:IEventHandler where  TEventDaTa:IEventData
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
       void  Handler(TEventDaTa eventData);
    }
}
