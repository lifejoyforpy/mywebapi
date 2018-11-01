using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus.Handlers
{ 

    /// <summary>
    /// 泛型事件处理器接口
    /// </summary>
    /// <typeparam name="TEventData"></typeparam>
   public interface IEventHandler< in TEventData>:IEventHandler where TEventData:IEventData
    {
        /// <summary>
        /// 所有处理事件的处理器
        /// </summary>
        /// <param name=""></param>
        void HandlerEvent(TEventData eventData);
    }
}
