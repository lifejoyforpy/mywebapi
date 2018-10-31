﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    public class EventBusIocManager
    {
        public readonly IWindsorContainer _container;
        public EventBusIocManager()
        {
            _container = new WindsorContainer();
        }
        /// <summary>
        /// 注册所有按照约定的事件和事件源绑定到容器里
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assembly"></param>
        /// <param name="lifetime"></param>
        public void RegisterByConvention(IWindsorContainer container, Assembly assembly, Lifetime lifetime = Lifetime.Tansient)
        {
            switch (lifetime)
            {
                case Lifetime.Tansient:
                    container.Register(Types.FromAssembly(assembly).BasedOn(typeof(IEventHandler)).LifestyleTransient());
                    break;
                case Lifetime.Scope:
                    container.Register(Types.FromAssembly(assembly).BasedOn(typeof(IEventHandler)).LifestyleScoped());
                    break;
                default:
                    container.Register(Types.FromAssembly(assembly).BasedOn(typeof(IEventHandler)).LifestyleSingleton());
                    break;
            }
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
