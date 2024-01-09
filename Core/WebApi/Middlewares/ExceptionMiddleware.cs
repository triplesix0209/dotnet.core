using System.Reflection;
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

        /// <summary>
        /// Exception middleware.
        /// </summary>
        /// <param name="next">Next action.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _webApiAppsetting = new WebApiAppsetting(configuration);
        }

        /// <summary>
        /// Middleware process.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <returns>Task process.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BaseException e)
            {
                await SendResponse(httpContext, e.ToErrorResult(httpContext));
                throw;
            }
            catch (AggregateException e)
            {
                ErrorResult error;
                if (e.InnerExceptions.Count == 1 && e.InnerException is BaseException exception)
                    error = exception.ToErrorResult(httpContext);
                else
                    error = ConvertExceptionToErrorResult(e);

                await SendResponse(httpContext, error);
                throw;
            }
            catch (TargetInvocationException e)
            {
                ErrorResult error;
                if (e.InnerException is BaseException exception)
                    error = exception.ToErrorResult(httpContext);
                else
                    error = ConvertExceptionToErrorResult(e);

                await SendResponse(httpContext, error);
                throw;
            }
            catch (Exception e)
            {
                var error = ConvertExceptionToErrorResult(e);
                await SendResponse(httpContext, error);
                throw;
            }
        }

        private ErrorResult ConvertExceptionToErrorResult(Exception exception)
        {
            var error = new ErrorResult(500, "exception", exception.Message, stackTrace: exception.StackTrace);

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                if (innerException.Message.IsNotNullOrEmpty())
                    error.Error.Message += $" {innerException.Message}";
                innerException = innerException.InnerException;
            }

            return error;
        }

        private async Task SendResponse(HttpContext httpContext, ErrorResult error)
        {
            var json = _webApiAppsetting.ShowStackTrace ?
                error.ToJson() :
                error.ToJson(nameof(ErrorResult.StackTrace));

            if (_webApiAppsetting.AllowedOrigins.Contains("*"))
                httpContext.Response.Headers.AccessControlAllowOrigin = "*";
            else if (httpContext.Request.Headers.Origin.Any() && _webApiAppsetting.AllowedOrigins.Contains(httpContext.Request.Headers.Origin.First()))
                httpContext.Response.Headers.AccessControlAllowOrigin = httpContext.Request.Headers.Origin.First();
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = error.HttpStatusCode;
            await httpContext.Response.WriteAsync(json!);
            await httpContext.Response.CompleteAsync();
        }
    }
}
