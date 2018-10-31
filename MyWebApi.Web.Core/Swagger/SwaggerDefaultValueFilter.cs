using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebApi.Web.Core.Swagger
{
    /// <summary>
    ///  版本控制默认参数接口实现 SwaggerDefaultValueFilter
    /// </summary>
    public class SwaggerDefaultValueFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = context.ApiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.Name);
                if (description == null)
                    return;
                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata.Description;
                }

                if (description.RouteInfo != null)
                {
                    parameter.Required |= !description.RouteInfo.IsOptional;
                    if (parameter.Default == null)
                        parameter.Default = description.RouteInfo.DefaultValue;
                }
            }
        }
    }
}
