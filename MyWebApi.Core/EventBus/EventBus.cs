using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MyWebApi.Core.EventBus.EventStore;
using MyWebApi.Core.EventBus.Handlers;
using MyWebApi.Core.EventBus.Handlers.Internals;

namespace MyWebApi.Core.EventBus
{
    /// <summary>
    /// evneybus implement
    /// </summary>
    public class EventBus : IEventBus
    {

        public IEventStore default ;
        public IWindsorContainer IocContainer { get; private set; }
        public EventBus()
        {
             IocContainer = IocContainer ?? new WindsorContainer();
            _handlerDict = new ConcurrentDictionary<Type, List<Type>>();
        }
        /// <summary>
        /// 事件源和事件绑定注册到容器
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerType"></param>
        public void Register(Type eventType, Type handlerType)
        {  
           //IocContainer.Register(Component.For) 
        }
        public void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            //1.构造ActionEventHandler
            var actionHandler = new ActionEventHandler<TEventData>(action);
            //2.将ActionEventHandler的实例注入到Ioc容器
            IocContainer.Register(Component.For<IEventHandler<TEventData>>().UsingFactoryMethod(() => actionHandler));
            //注册事件到事件总线
            this.Register(actionHandler);
        }
        public void Register<TEventData>(IEventHandler<TEventData> eventHandler) where TEventData : IEventData
        {
            //Register(TEventData, eventHandler);
        }
        public void Register<TEventData, TEventHandler>()
           where TEventData : IEventData
           where TEventHandler : IEventHandler, new()
        {
            this.Register(typeof(TEventData), typeof(TEventHandler));
        }
        /// <summary>
        /// eventType 数据源类型，IEventHandler 事件处理器
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void Register(Type eventType, IEventHandler handler)
        {
            this.Register(eventType, handler.GetType());
        }


        public void AsyncUnregister<TEventData>(Func<TEventData, Task> action) where TEventData : IEventData
        {
           
        }

        public void AsyncUnregister<TEventData>(IAsyncEventHandler<TEventData> handler) where TEventData : IEventData
        {
           
        }

       

        

       
        public IDisposable RegisterAsync<TEventData>(Func<TEventData, Task> func) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public IDisposable RegisterAsync<TEventData>(IAsyncEventHandler<TEventData> eventHandler) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type eventType)
        {
            throw new NotImplementedException();
        }
    }
}
