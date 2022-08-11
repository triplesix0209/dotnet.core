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
                await SendResponse(httpContext, e.ToErrorResult(httpContext));
                throw;
            }
            catch (AggregateException e)
            {
                ErrorResult error;
                if (e.InnerExceptions.Count == 1 && e.InnerException is BaseException exception)
                    error = exception.ToErrorResult(httpContext);
                else
                    error = ConvertExceptionToErrorResult(httpContext, e);

                await SendResponse(httpContext, error);
                throw;
            }
            catch (TargetInvocationException e)
            {
                ErrorResult error;
                if (e.InnerException is BaseException exception)
                    error = exception.ToErrorResult(httpContext);
                else
                    error = ConvertExceptionToErrorResult(httpContext, e);

                await SendResponse(httpContext, error);
                throw;
            }
            catch (Exception e)
            {
                var error = ConvertExceptionToErrorResult(httpContext, e);
                await SendResponse(httpContext, error);
                throw;
            }
        }

        private ErrorResult ConvertExceptionToErrorResult(HttpContext httpContext, Exception exception)
        {
            var error = new ErrorResult(httpContext.Response.StatusCode, "exception", exception.Message);

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                error.Detail.Add(innerException.Message);
                innerException = innerException.InnerException;
            }

            return error;
        }

        private async Task SendResponse(HttpContext httpContext, ErrorResult error)
        {
            var json = _webApiAppsetting.ShowErrorDetail ?
                error.ToJson() :
                error.ToJson(nameof(ErrorResult.Detail));

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = error.HttpStatusCode;
            await httpContext.Response.WriteAsync(json!);
            await httpContext.Response.CompleteAsync();
        }
    }
}
