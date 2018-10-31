using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyWebApi.Web.Core.Swagger
{
    /// <summary>
    /// 自定义中间件
    /// </summary>
   public static class SwaggerBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            return UseCustomSwagger(app, new CustsomSwaggerOptions());
        }
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, CustsomSwaggerOptions options)
        {
            app.UseSwagger(opt =>
            {
                if (options.UseSwaggerAction == null) return;
                    options.UseSwaggerAction(opt);
            });
            app.UseSwaggerUI(c =>
            {
                if (options.ApiVersions == null) return;
                c.RoutePrefix = options.RoutePrefix;
                c.DocumentTitle = options.ProjectName;
                if (options.UseCustomIndex)
                {
                    c.UseCustomSwaggerIndex();
                }
                foreach (var item in options.ApiVersions)
                {
                    c.SwaggerEndpoint($"/swagger/{item}/swagger.json", $"{item}");
                }
                options.UseSwaggerUIAction?.Invoke(c);
            });

            return app;
        }
        /// <summary>
        /// 使用自定义首页
        /// </summary>
        /// <returns></returns>
        public static void UseCustomSwaggerIndex(this SwaggerUIOptions c)
        {
            var currentAssembly = typeof(CustsomSwaggerOptions).GetTypeInfo().Assembly;
            c.IndexStream = () => currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.index.html");
        }
    }
}
