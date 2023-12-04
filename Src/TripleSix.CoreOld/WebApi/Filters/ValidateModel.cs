using Microsoft.AspNetCore.Mvc.Filters;
using TripleSix.CoreOld.WebApi.Results;

namespace TripleSix.CoreOld.WebApi.Filters
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