using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public IEventStore _eventStroe ;
        public IWindsorContainer IocContainer { get; private set; }

        public static EventBus Default { get; private set; }


        static EventBus()
        {
            Default = new EventBus();
        }
        public EventBus()
        {
             IocContainer = IocContainer ?? new WindsorContainer();
            _eventStroe = _eventStroe?? new InMemoryEventStore();
        }
        #region register
        public void Register<TEventData>(IEventHandler eventHandler) where TEventData : IEventData
        {
            Register(typeof(TEventData), eventHandler.GetType());
        }
        public void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            //1.构造ActionEventHandler
            var actionHandler = new ActionEventHandler<TEventData>(action);
            //2.将ActionEventHandler的实例注入到Ioc容器
            IocContainer.Register(
                Component.For<IEventHandler<TEventData>>()
                .UsingFactoryMethod(() => actionHandler));
            //注册事件总线

            //3.注册到事件总线
            Register<TEventData>(actionHandler);
        }
        public void Register(Type eventType, Type handler)
        {
           var handlerInterface= handler.GetInterface("IEventHandler`1");
            if (!IocContainer.Kernel.HasComponent(handlerInterface))
            {
                IocContainer.Register(Component
                    .For(handlerInterface, handler));
            }
            _eventStroe.AddRegister(eventType, handler);
        }
        /// <summary>
        ///注册所有的 
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterAllEventHandlerFromAssembly(Assembly assembly)
        {
            //将所有IEventhandler注册
            IocContainer.Register(Classes.
                FromAssembly(assembly)
                .BasedOn(typeof(IEventHandler<>))
                .WithService.Base());
            //获取所有在ioc注册的IEventHandler

            var handlers=IocContainer.Kernel.GetAssignableHandlers(typeof(IEventHandler));
            foreach (var handler in handlers)
            {
               var interfaces=  handler.ComponentModel.Implementation.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (typeof(IEventHandler).IsAssignableFrom(@interface))
                    {
                        continue;
                    }

                    var genericArgs = @interface.GetGenericArguments();
                    this.Register(genericArgs[0], @interface);
                }
            }

        }
        #endregion
        #region unregister
        /// <summary>
        ///  手动解除事件源与事件处理的绑定
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="handlerType"></param>
        public void UnRegister<TEventData>(Type handlerType) where TEventData : IEventData
        {
            _eventStroe.RemoveRegister(typeof(TEventData), handlerType);
        }
        public void UnRegisterAll<TEventData>() where TEventData : IEventData
        {
            //获取所有映射的EventHandler
            List<Type> handlerTypes = _eventStroe.GetHandlersForEvent(typeof(TEventData)).ToList();
            foreach (var handlerType in handlerTypes)
            {
                _eventStroe.RemoveRegister(typeof(TEventData), handlerType);
            }
        }
        #endregion
        #region triggert
        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            //获取所有映射的EventHandler
            List<Type> handlerTypes = _eventStroe.GetHandlersForEvent(eventData.GetType()).ToList();
            if (handlerTypes.Count > 0)
            {
                foreach (var handlerType in handlerTypes)
                {
                    //从Ioc容器中获取所有的实例
                    var handlerInterface = handlerType.GetInterface("IEventHandler`1");
                    var eventHandlers = IocContainer.ResolveAll(handlerInterface);
                    //循环遍历，仅当解析的实例类型与映射字典中事件处理类型一致时，才触发事件
                    foreach (var eventHandler in eventHandlers)
                    {
                        if (eventHandler.GetType() == handlerType)
                        {
                          var  handler=  eventHandler as IEventHandler<IEventData>;
                            handler?.HandlerEvent(eventData);
                        }
                    }
                }
            }
        }
        public void Trigger<TEventData>(Type eventHandlerType, TEventData eventData) where TEventData : IEventData
        {
            if (_eventStroe.HasRegisterForEvent<TEventData>())
            {
                var handlers = _eventStroe.GetHandlersForEvent<TEventData>();
                if (handlers.Any(th => th == eventHandlerType))
                {
                    //获取类型实现的泛型接口
                    var handlerInterface = eventHandlerType.GetInterface("IEventHandler`1");
                    var eventHandlers = IocContainer.ResolveAll(handlerInterface);
                    //循环遍历，仅当解析的实例类型与映射字典中事件处理类型一致时，才触发事件
                    foreach (var eventHandler in eventHandlers)
                    {
                        if (eventHandler.GetType() == eventHandlerType)
                        {
                            var handler = eventHandler as IEventHandler<TEventData>;
                            handler?.HandlerEvent(eventData);
                        }
                    }
                }
            }
             
        }

        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return Task.Run(() => Trigger<TEventData>(eventData));
        }

        public Task TriggerAsycn<TEventData>(Type eventHandlerType, TEventData eventData) where TEventData : IEventData
        {
            return Task.Run(() => Trigger(eventHandlerType, eventData));
        }
        #endregion
    }
}
