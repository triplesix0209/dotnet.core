﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class TimestampModelBinder : IModelBinder
    {
        /// <inheritdoc/>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (values.Length == 0 || values.FirstValue == null) return Task.CompletedTask;

            var result = long.Parse(values.FirstValue).ToDateTime();

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }

    public class TimestampModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc/>
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
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
