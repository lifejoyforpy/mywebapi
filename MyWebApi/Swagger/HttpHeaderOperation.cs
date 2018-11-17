using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyWebApi.Swagger
{
    public class HttpHeaderOperation : IOperationFilter
    {
        private MethodInfo actionAttr;

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
             
            context.ApiDescription.TryGetMethodInfo(out actionAttr);
            var isNeedAuthroized=  actionAttr.GetCustomAttributes().Any(a => a.GetType() == typeof(AuthorizeAttribute));
        }
    }
}
