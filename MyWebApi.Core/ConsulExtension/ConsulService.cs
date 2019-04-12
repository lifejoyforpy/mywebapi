using System.Linq;
using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using MyWebApi.Core.Utility;
using System.Net.NetworkInformation;

namespace MyWebApi.Core.ConsulExtension
{
    /// <summary>
    /// 注册的服务名称和ID
    /// </summary>
    public class ConsulConfig
    {
        public ConsulConfig()
        {
            ServiceName = "ServiceName";
            ServiceId = "ServiceId";
            Address = "127.0.0.1:8500";
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务Id
        /// </summary>
        public string ServiceId { get; set; }

        public string Address { get; set; }

    }
    public  static class ConsulService
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration,string key)
        {
            services.Configure<ConsulConfig>(configuration.GetSection(key));
            return services;
        }

        /// <summary>
        /// 注册 consulclient
        /// </summary>
        /// <typeparam name="ConsulConfig"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services,  Action<ConsulClientConfiguration> consulConfig)
        {
            //注入ConsulClient
            services.AddSingleton<IConsulClient, ConsulClient>(client => new ConsulClient(consulConfig
            ));      
            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app,IApplicationLifetime applicationLifetime)
        {
            //获取server ip address 指定ip和端口的才可以获取
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();
            //建立连接的client
            
            var ip= IpExtensions.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            var client =app.ApplicationServices.GetRequiredService<IConsulClient>();
            var consulConfig = app.ApplicationServices.GetRequiredService<IOptions<ConsulConfig>>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();
          
            var uri = new Uri(address);
            var healthchecks = new AgentServiceCheck()
            {
                Interval = TimeSpan.FromMinutes(1),
                HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/api/health/status"
            };
            var register = new AgentServiceRegistration
            {
                ID = consulConfig.Value.ServiceId,
                Name= consulConfig.Value.ServiceName,
                Address = $"{uri.Scheme}://{uri.Host}",
                Port = uri.Port,
                Check = healthchecks,
                Tags=new[] { "webapi"}
            };
            logger.LogInformation("Register with consul");
            client.Agent.ServiceRegister(register).Wait();
            
         //  client.Agent.ServiceDeregister(register.ID).Wait();
            //注入 应用生命周期结束的回调
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("DeRegister with consul");
                client.Agent.ServiceDeregister(register.ID).Wait();
            });
            return app;
        }
    }
}
