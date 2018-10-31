using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Web.Core.Swagger
{
   public static class CustomSwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            return AddCustomSwagger(services, new CustsomSwaggerOptions());
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, CustsomSwaggerOptions options)
        {
            services.AddSwaggerGen(c =>
            {
                if (options.ApiVersions == null) return;
                foreach (var version in options.ApiVersions)
                {
                    c.SwaggerDoc(version, new Info { Title = options.ProjectName, Version = version });
                }
                c.OperationFilter<SwaggerDefaultValueFilter>();
                options.AddSwaggerGenAction?.Invoke(c);

            });
            return services;
        }
    }
}
