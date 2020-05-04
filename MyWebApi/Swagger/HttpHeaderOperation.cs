using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyWebApi.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHeaderOperation : IOperationFilter
    {
        private MethodInfo actionAttr;
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            context.ApiDescription.TryGetMethodInfo(out actionAttr);
            var isNeedAuthroized = actionAttr.GetCustomAttributes().Any(a => a.GetType() == typeof(AuthorizeAttribute));
        }
    }
}
