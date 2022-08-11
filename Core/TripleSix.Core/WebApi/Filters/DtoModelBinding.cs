using System.Collections;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Model binding dto.
    /// </summary>
    public class DtoModelBinding : ActionFilterAttribute
    {
        private static readonly StringComparison _stringComparison = StringComparison.CurrentCultureIgnoreCase;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (!context.ActionArguments.ContainsKey(parameter.Name)) continue;
                var data = context.ActionArguments[parameter.Name];
                if (data == null || data is not IDto dto) continue;

                var bindingSource = parameter.BindingInfo?.BindingSource?.Id;
                if (bindingSource.IsNullOrWhiteSpace()) bindingSource = "Query";

                context.ActionArguments[parameter.Name] = await NormalizeParameter(request, bindingSource, dto);
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private async Task<IDto> NormalizeParameter(HttpRequest request, string bindingSource, IDto value)
        {
            var parameterProperties = value.GetType().GetProperties();

            bool propertyHaveInput;
            if (bindingSource == "Query")
            {
                foreach (var property in parameterProperties)
                {
                    if (property.PropertyType.IsArray || property.PropertyType.IsAssignableTo<IEnumerable>())
                    {
                        propertyHaveInput = request.Query.Keys.Any(x =>
                            x.Equals(property.Name, _stringComparison)
                            || x.StartsWith($"{property.Name}[", _stringComparison));
                    }
                    else if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                    {
                        propertyHaveInput = request.Query.Keys.Any(x =>
                            x.Equals(property.Name, _stringComparison)
                            || x.StartsWith($"{property.Name}.", _stringComparison));
                    }
                    else
                    {
                        propertyHaveInput = request.Query.Keys.Any(x =>
                            x.Equals(property.Name, _stringComparison));
                    }

                    if (propertyHaveInput)
                        value.SetPropertyChanged(property.Name, true);
                }
            }
            else if (bindingSource == "Body")
            {
                JObject? bodyData;
                request.EnableBuffering();
                using (var reader = new StreamReader(
                    request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    request.Body.Position = 0;
                    bodyData = (await reader.ReadToEndAsync()).ToJToken() as JObject;
                    request.Body.Position = 0;
                }

                if (bodyData == null) return value;
                var bodyProperties = bodyData.Properties();
                foreach (var property in parameterProperties)
                {
                    if (bodyProperties.Any(x => x.Name.Equals(property.Name, _stringComparison)))
                        value.SetPropertyChanged(property.Name, true);
                }
            }

            return value;
        }
    }
}
