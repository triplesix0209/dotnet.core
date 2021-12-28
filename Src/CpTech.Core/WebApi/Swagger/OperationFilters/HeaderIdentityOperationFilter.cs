using System.Linq;
using System.Text;
using CpTech.Core.Dto;
using CpTech.Core.Enums;
using CpTech.Core.Extensions;
using CpTech.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CpTech.Core.WebApi.Swagger
{
    public class HeaderIdentityOperationFilter<TIdentity> : IOperationFilter
        where TIdentity : IIdentity
    {
        public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
                return;

            var description = new StringBuilder("phân loại thiết bị");
            EnumHelper.GetValues<ClientDeviceType>().ToList().ForEach(value =>
            {
                description
                    .Append("<br/>")
                    .Append(value)
                    .Append(" = ")
                    .Append(EnumHelper.GetDescription(value));
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = "client-device-type",
                Description = description.ToString(),
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Nullable = true,
                },
            });
        }
    }
}