using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.Core.EventBus.Handlers.Internals
{
    /// <summary>
    /// 支持直接注入处理action的异步处理器
    /// </summary>
    /// <typeparam name="TEventData"></typeparam>
    internal class AsyncActionEventHandler<TEventData> : IAsyncEventHandler<TEventData> where TEventData : IEventData
    {
        /// <summary>
        /// 处理器
        /// </summary>
        public Action<TEventData> Action { get; private set; }
        public AsyncActionEventHandler(Action<TEventData> action)
        {
            Action = action;
        }
        /// <summary>
        /// 异步处理器
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandlerEventAsync(TEventData eventData)
        {
             await  Task.Run(()=> Action(eventData));
        }

       
    }
}
