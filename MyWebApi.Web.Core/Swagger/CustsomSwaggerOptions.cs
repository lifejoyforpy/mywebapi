using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Web.Core.Swagger
{
    /// <summary>
    /// 构建参数模型
    /// </summary>
    public class CustsomSwaggerOptions
    {  
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 接口文档显示版本
        /// </summary>
        /// 
        public string[] ApiVersions { get; set; }
        /// <summary>
        /// 接口文档访问路由前缀
        /// </summary>
        public string RoutePrefix { get; set; }
        /// <summary>
        /// 使用自定义首页
        /// </summary>
        public bool UseCustomIndex { get; set; }
        /// <summary>
        /// UserSwagger Hook
        /// </summary>
        public Action<SwaggerOptions> UseSwaggerAction { get; set; }

        /// <summary>
        /// UseSwaggerUI Hook
        /// </summary>
        public Action<SwaggerUIOptions> UseSwaggerUIAction { get; set; }
        /// <summary>
        /// AddSwaggerGen Hook
        /// </summary>
        public Action<SwaggerGenOptions> AddSwaggerGenAction { get; set; }
    }
}
