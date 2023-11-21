using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;
using TripleSix.Core.Types;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Tự động kiểm tra input của request.
    /// </summary>
    public class ValidateInput : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = new List<InputInvalidItem>();
            foreach (var input in context.ActionArguments)
            {
                if (input.Value is not IDto inputValue) continue;

                var validationResult = inputValue.Validate(httpContext: context.HttpContext);
                foreach (var error in validationResult.Errors)
                {
                    var propertyName = error.PropertyName.Split('.')
                        .Select(x => x.ToCamelCase())
                        .ToString(".");
                    errors.Add(new()
                    {
                        FieldName = propertyName,
                        ErrorCode = error.ErrorCode.ToSnakeCase(),
                        ErrorMessage = error.ErrorMessage,
                    });
                }
            }

            if (!errors.Any())
            {
                foreach (var field in context.ModelState)
                {
                    if (field.Value.ValidationState != ModelValidationState.Invalid)
                        continue;

                    foreach (var error in field.Value.Errors)
                    {
                        if (field.Key.IsNotNullOrEmpty() && error.ErrorMessage.EndsWith("is required."))
                            continue;

                        errors.Add(new InputInvalidItem
                        {
                            FieldName = field.Key.IsNullOrEmpty() ? "input" : field.Key,
                            ErrorCode = "model_validator",
                            ErrorMessage = error.ErrorMessage,
                        });
                    }
                }
            }

            if (!errors.Any()) return;
            context.Result = new InputInvalidException(errors).ToErrorResult();
        }
    }
}
