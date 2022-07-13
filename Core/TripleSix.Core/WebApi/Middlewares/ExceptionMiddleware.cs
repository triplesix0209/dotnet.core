using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Appsettings;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Middleware xử lý exception.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebApiAppsetting _webApiAppsetting;

        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _webApiAppsetting = new WebApiAppsetting(configuration);
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BaseException e)
            {
                await HandleBaseException(httpContext, e);
                throw;
            }
            catch (Exception e)
            {
                await HandleUnexpectedException(httpContext, e);
                throw;
            }
        }

        private async Task HandleBaseException(HttpContext context, BaseException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.HttpCodeStatus;

            var result = new ErrorResult(exception.HttpCodeStatus, exception.Code, exception.Message);
            await context.Response.WriteAsync(JsonHelper.SerializeObject(result));
            await context.Response.CompleteAsync();
        }

        private async Task HandleUnexpectedException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var message = "internal server error";
            if (_webApiAppsetting.DisplayUnexpectedException)
            {
                var messages = new List<string>();
                var currentException = exception;
                while (currentException != null)
                {
                    messages.Add(currentException.Message);
                    currentException = currentException.InnerException;
                }

                message = messages.ToString(" ");
            }

            var result = new ErrorResult(context.Response.StatusCode, "exception", message);
            await context.Response.WriteAsync(JsonHelper.SerializeObject(result));
            await context.Response.CompleteAsync();
        }
    }
}
