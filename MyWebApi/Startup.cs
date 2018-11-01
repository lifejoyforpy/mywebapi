using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyWebApi.EntityFramework;
using MyWebApi.EntityFramework.UnitOfWork;
using MyWebApi.Web.Core.Logger;
using MyWebApi.Web.Core.Swagger;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace MyWebApi
{/// <summary>
/// 
/// </summary>
    public class Startup
    {  /// <summary>
    /// 
    /// </summary>
        private CustsomSwaggerOptions CURRENT_SWAGGER_OPTIONS=new CustsomSwaggerOptions()
        {

            ProjectName = "Mywebapi接口",
                ApiVersions = new string[] { "v1", "v2" },//要显示的版本
                UseCustomIndex = true,
                RoutePrefix = "swagger",
                AddSwaggerGenAction = c =>
                {
                    var filePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml");
                    c.IncludeXmlComments(filePath, true);
                },
                UseSwaggerAction = c =>
                {

                },
                UseSwaggerUIAction = c =>
                {

                }
            };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
          //  SerilogConfiguration.CreateLogger();
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //添加返回结果xml格式
            //.AddMvcOptions(options=> {
            //    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            //});
            //版本控制

            //注入dbcontext
            services.AddDbContext<MyContext>(options=> {
                options.UseSqlServer(Configuration.GetSection("connectionStrings:default").Value);
            });
            //用扩展方法注入uow
            services.AddUnitOfWork<MyContext>();
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddApiVersioning(option =>
            {
                // allow a client to call you without specifying an api version
                // since we haven't configured it otherwise, the assumed api version will be 1.0
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ReportApiVersions = false;
            });
            ////custom swagger
            //services.AddCustomSwagger(CURRENT_SWAGGER_OPTIONS);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            //添加Cors，并配置CorsPolicy 
            services.AddCors(options => options.AddPolicy("CorsTest", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            //日志
            services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //配置日志
         //    loggerFactory.AddSerilog();
            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            //custom swagger
            //自动检测存在的版本
            // CURRENT_SWAGGER_OPTIONS.ApiVersions = provider.ApiVersionDescriptions.Select(s => s.GroupName).ToArray();
            //app.UseCustomSwagger(CURRENT_SWAGGER_OPTIONS);
            //注意UseCors()要在UseMvc()之前
            //  app.UseCors("CorsTest");
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
