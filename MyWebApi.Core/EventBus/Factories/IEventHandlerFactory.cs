using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus.Factories
{
    /// <summary>
    /// 时间处理器工厂
    /// </summary>
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// 获取事件处理器
        /// </summary>
        /// <returns></returns>
        IEventHandler GetHandler();
        /// <summary>
        ///  Gets type of the handler (without creating an instance).
        /// </summary>
        /// <returns></returns>
        Type GetHandlerType();
        /// <summary>
        /// Releases an event handler.
        /// </summary>
        /// <param name="handler"></param>
        void ReleaseHandler(IEventHandler handler);
    }
}
