using Microsoft.AspNetCore.Mvc.Filters;
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

            if (!errors.Any()) return;
            context.Result = new InputInvalidException(errors).ToErrorResult();
        }
    }
}
