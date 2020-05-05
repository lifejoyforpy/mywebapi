using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<EventQueue.EventQueue>();
            services.AddSingleton<EventStore.EventStore>();
            services.AddSingleton<IEventPublish, EventPublish>();
            services.AddSingleton<IEventSubscriptionManager, EventSubscriptionManager>();
            services.AddSingleton<IHostedService, EventConsumer>();
            return services;
        }
    }
}
