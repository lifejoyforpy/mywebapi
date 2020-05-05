using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    public static class EventBusExtension
    {

        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.TryAddSingleton<EventStore.EventStore>();
            services.TryAddSingleton<EventQueue.EventQueue>();         
            services.TryAddSingleton<IEventPublish, EventPublish>();
            services.TryAddSingleton<IEventSubscriptionManager, EventSubscriptionManager>();
             services.AddHostedService<EventConsumer>();
            return services;
        }
    }
}
