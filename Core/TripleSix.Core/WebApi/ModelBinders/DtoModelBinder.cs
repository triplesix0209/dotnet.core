#pragma warning disable SA1402 // File may only contain a single type

using System.Text;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    public class DtoModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;

            if (bindingContext.BindingSource == BindingSource.Body)
            {
                var request = bindingContext.HttpContext.Request;
                request.EnableBuffering();
                using (var reader = new StreamReader(
                    request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    var bodyContent = await reader.ReadToEndAsync();
                    var bodyProperties = (bodyContent.ToJToken() as JObject)?.Properties();
                    request.Body.Position = 0;
                    if (bodyContent.ToObject(modelType) is not IDto model
                        || bodyProperties == null)
                        return;

                    foreach (var property in modelType.GetProperties())
                    {
                        if (bodyProperties.Any(x => x.Name.ToLower() == property.Name.ToLower()))
                            model.SetPropertyChanged(property.Name, true);
                    }

                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
            else
            {
                var jObject = new JObject();
                foreach (var property in modelType.GetProperties())
                {
                    var values = bindingContext.ValueProvider.GetValue(property.Name);
                    if (!values.Any()) continue;
                    jObject.Add(property.Name, values.FirstValue);
                }

                var model = jObject.ToObject(modelType) as IDto;
                foreach (var propertyName in jObject.Properties().Select(x => x.Name))
                    model?.SetPropertyChanged(propertyName, true);

                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
    }

    public class DtoModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!context.Metadata.ModelType.IsAssignableTo<IDto>())
                return null;

            return new DtoModelBinder();
        }
    }
}
