#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Threading.Tasks;
using TripleSix.Core.DataTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TripleSix.Core.WebApi.ModelBinders
{
    public class PhoneModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (values.Length == 0) return Task.CompletedTask;

            var result = new Phone(values.FirstValue);

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }

    public class PhoneModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType != typeof(Phone))
                return null;

            return new PhoneModelBinder();
        }
    }
}