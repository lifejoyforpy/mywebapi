using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus.Handlers.Internals
{
    internal class ActionEventHandler< TEventData>:IEventHandler<TEventData> where TEventData:IEventData
    {
        public Action<TEventData> Action { get; private set; }
        public ActionEventHandler(Action<TEventData> handler)
        {
            Action = handler;
        }

        public void HandlerEvent(TEventData eventData)
        {
            Action(eventData);
        }
    }
}
