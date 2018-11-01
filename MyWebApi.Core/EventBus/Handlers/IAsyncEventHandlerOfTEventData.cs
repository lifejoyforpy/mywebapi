using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.Core.EventBus.Handlers
{
   public interface IAsyncEventHandler<in TEventData>:IEventHandler where TEventData:IEventData
    {
        Task HandlerEventAsync(TEventData eventData);
    }
}
