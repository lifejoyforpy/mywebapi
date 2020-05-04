using System;

using System.Reflection;

using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyWebApi.Core.ConsulExtension;
using MyWebApi.EntityFramework;
using MyWebApi.EntityFramework.UnitOfWork;
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
            _Configuration = configuration;
          //  SerilogConfiguration.CreateLogger();
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration _Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.EnableEndpointRouting = false; });
            //添加返回结果xml格式
            //.AddMvcOptions(options=> {
            //    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            //});
            //版本控制

            //注入dbcontext
            services.AddDbContext<MyContext>(options=> {
                options.UseSqlServer(_Configuration.GetSection("connectionStrings:default").Value);
            });
            //用扩展方法注入uow
            services.AddUnitOfWork<MyContext>();
            services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddApiVersioning(option =>
            {
                // allow a client to call you without specifying an api version
                // since we haven't configured it otherwise, the assumed api version will be 1.0
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ReportApiVersions = false;
            });
            ////custom swagger
            //services.AddCustomSwagger(CURRENT_SWAGGER_OPTIONS);
      

            services.AddMvcCore().AddAuthorization();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http//localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "Test";
                });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            //添加Cors，并配置CorsPolicy 
            services.AddCors(options => options.AddPolicy("CorsTest", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            //日志
            services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));
            //请求id
            services.AddCorrelationId();
            //
            services.AddConsulConfig(_Configuration, "consulConfig").AddConsul(config=> {
                config.Address = new Uri (_Configuration["consulConfig:Address"]);              
            });
            //services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>((options) =>
            //{
            //    var configString = _Configuration.GetSection("RedisConfig:default").Value;           
            //    return ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(configString));
            //});
            ////基于托管的后台任务的redis队列
            //services.AddHostedService<RedisListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="applicationLifetime"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IHostApplicationLifetime applicationLifetime)
        {
            app.UseCorrelationId();
            app.RegisterConsul(applicationLifetime);
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
            // static file
            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.InjectJavascript("/swagger/ui/swagger-ui.js");
                c.InjectStylesheet("/swagger/ui/custom.css");
            });
            //custom swagger
            //自动检测存在的版本
            // CURRENT_SWAGGER_OPTIONS.ApiVersions = provider.ApiVersionDescriptions.Select(s => s.GroupName).ToArray();
            //app.UseCustomSwagger(CURRENT_SWAGGER_OPTIONS);
            //注意UseCors()要在UseMvc()之前
            //  app.UseCors("CorsTest");
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
