using Microsoft.AspNetCore.Mvc.Filters;
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
            var errors = new List<ErrorItem>();
            foreach (var input in context.ActionArguments)
            {
                if (input.Value is IDto inputValue)
                {
                    var validationResult = inputValue.Validate(httpContext: context.HttpContext);
                    foreach (var error in validationResult.Errors)
                    {
                        errors.Add(new()
                        {
                            PropertyName = error.PropertyName.ToCamelCase(),
                            ErrorCode = error.ErrorCode.ToSnakeCase(),
                            ErrorMessage = error.ErrorMessage
                        });
                    }
                }
            }

            if (!errors.Any()) return;
            context.Result = new ErrorResult(400, "request_input_invalid", "Thông tin đầu vào bị thiếu hoặc sai", errors);
        }

        private class ErrorItem
        {
            public string PropertyName { get; set; }

            public string ErrorCode { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}
