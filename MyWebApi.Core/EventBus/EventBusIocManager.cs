using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MyWebApi.Core.EventBus.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    public class EventBusIocManager
    {
        public readonly IWindsorContainer _container;

        private ConcurrentDictionary<Type, List<Type>> _eventMapping;
        public EventBusIocManager()
        {
            _container = new WindsorContainer();
            _eventMapping = _eventMapping??new ConcurrentDictionary<Type, List<Type>>();
        }
        /// <summary>
        /// 注册所有按照约定的事件和事件源绑定到容器里
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assembly"></param>
        /// <param name="lifetime"></param>
        public void RegisterByConvention(IWindsorContainer container, Assembly assembly)
        {
            //将IEventHandler注入到程序集
            _container.Register(Classes.FromAssembly(assembly).BasedOn(typeof(IEventHandler<>)).WithService.Base());
            //从ioc容易中获取所有注入的
            _container.Kernel.GetAssignableHandlers(typeof(IEventHandler));
        }

        /// <summary>
        /// 手动绑定事件源和事件
        /// </summary>
        /// <typeparam name="TEvenData"></typeparam>
        /// <param name="handlerType"></param>
        public void Register<TEvenData>(Type handlerType) where TEvenData :IEventData
        {
            var handlerInterface = handlerType.GetInterface("IEventHandler`1");
            if (!_container.Kernel.HasComponent(handlerInterface))
            {
                _container.Kernel.Register(Component.For(handlerInterface,handlerType));
            }
        }
        public void UnRegister<TEvenData>(Type handlerType) where TEvenData : IEventData
        {
          
        }
    }
}
