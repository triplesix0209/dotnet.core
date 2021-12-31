using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.WebApi.Filters
{
    public class ValidateModel : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            context.Result = new BadRequestErrorResult(context.ModelState);
        }
    }
}