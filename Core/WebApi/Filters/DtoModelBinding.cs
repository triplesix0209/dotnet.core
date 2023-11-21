using System.Collections;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using TripleSix.Core.Exceptions;
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

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (!context.ActionArguments.ContainsKey(parameter.Name)) continue;
                var data = context.ActionArguments[parameter.Name];
                if (data == null || data is not IDto dto) continue;

                var bindingSource = parameter.BindingInfo?.BindingSource?.Id;
                if (bindingSource.IsNullOrEmpty()) bindingSource = "Query";

                context.ActionArguments[parameter.Name] = await NormalizeParameter(request, bindingSource, dto);
            }

            if (context.ActionArguments.Count == 0 && !context.ModelState.IsValid)
            {
                var errors = new List<InputInvalidItem>();
                foreach (var key in context.ModelState.Keys)
                {
                    var data = context.ModelState[key];
                    if (data == null) continue;
                    foreach (var error in data.Errors)
                    {
                        errors.Add(new()
                        {
                            FieldName = key,
                            ErrorCode = "model_validator",
                            ErrorMessage = error.ErrorMessage,
                        });
                    }
                }

                throw new InputInvalidException(errors);
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private static async Task<IDto> NormalizeParameter(HttpRequest request, string bindingSource, IDto value)
        {
            bool propertyHaveInput;
            if (bindingSource == "Query")
            {
                foreach (var property in value.GetType().GetProperties())
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
                SetBodyPropertyChanged(value, bodyData);
            }

            return value;
        }

        private static void SetBodyPropertyChanged(IDto result, JObject bodyData)
        {
            var resultProperties = result.GetType().GetProperties();
            var bodyProperties = bodyData.Properties();
            foreach (var resultProperty in resultProperties)
            {
                var bodyProperty = bodyProperties.FirstOrDefault(x => x.Name.Equals(resultProperty.Name, _stringComparison));
                if (bodyProperty == null) continue;

                result.SetPropertyChanged(resultProperty.Name, true);

                if (resultProperty.GetValue(result) is IDto childResult
                    && bodyProperty.Value is JObject childBodyProperty)
                    SetBodyPropertyChanged(childResult, childBodyProperty);
            }
        }
    }
}
