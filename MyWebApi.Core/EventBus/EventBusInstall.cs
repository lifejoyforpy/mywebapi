using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    /// <summary>
    ///  Installs event bus system and registers all handlers automatically.
    /// </summary>
    public class EventBusInstall: IWindsorInstaller
    {
        private IEventBus eventBus;
        private ConcurrentDictionary<Type, List<Type>> _handlerDict;
                                   //public EventBusInstall()
                                   //{
                                   //    _handlerDict=
        //}

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            throw new NotImplementedException();
        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            /* This code checks if registering component implements any IEventHandler<TEventData> interface, if yes,
            * gets all event handler interfaces and registers type to Event Bus for each handling event.
            */
            if(!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }
            var @interfaces= handler.ComponentModel.Implementation.GetTypeInfo().GetInterfaces();
            foreach (var @interface in @interfaces)
            {

            }
        }
    }
}
