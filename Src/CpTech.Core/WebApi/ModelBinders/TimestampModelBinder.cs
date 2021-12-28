#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Threading.Tasks;
using CpTech.Core.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CpTech.Core.WebApi.ModelBinders
{
    public class TimestampModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (values.Length == 0) return Task.CompletedTask;

            var result = DateTimeHelper.ParseEpochTimestamp(long.Parse(values.FirstValue));

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }

    public class TimestampModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType != typeof(DateTime) &&
                context.Metadata.ModelType != typeof(DateTime?))
                return null;

            return new TimestampModelBinder();
        }
    }
}