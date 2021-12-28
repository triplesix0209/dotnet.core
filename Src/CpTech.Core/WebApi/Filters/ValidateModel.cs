using CpTech.Core.WebApi.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CpTech.Core.WebApi.Filters
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