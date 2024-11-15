using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// OpenTelemetry Tracing.
    /// </summary>
    public class OpenTelemetryTracing : TypeFilterAttribute
    {
        /// <summary>
        /// OpenTelemetry Tracing.
        /// </summary>
        public OpenTelemetryTracing()
            : base(typeof(OpenTelemetryTracingImplement))
        {
        }
    }

    internal class OpenTelemetryTracingImplement : ActionFilterAttribute
    {
        public OpenTelemetryTracingImplement(ILogger<OpenTelemetryTracing> logger)
        {
            Logger = logger;
        }

        public ILogger<OpenTelemetryTracing> Logger { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await next();

            var activity = Activity.Current;
            if (activity == null) return;

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                activity.SetTag("api.controller", controllerActionDescriptor.ControllerName);
                activity.SetTag("api.action", controllerActionDescriptor.ActionName);
            }

            activity.SetTag("api.response", context.Result.ToJson());
        }
    }
}
