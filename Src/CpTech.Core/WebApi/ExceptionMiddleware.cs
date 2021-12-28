using System;
using System.Threading.Tasks;
using CpTech.Core.Exceptions;
using CpTech.Core.JsonSerializers;
using CpTech.Core.WebApi.Results;
using Microsoft.AspNetCore.Http;

namespace CpTech.Core.WebApi
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
            }
            catch (Exception e)
            {
                await HandleUnexpectedException(httpContext, e);
            }
        }

        private async Task HandleBaseException(HttpContext context, BaseException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.HttpCode;

            var result = new ErrorResult<object>(
                exception.HttpCode,
                exception.Code,
                exception.Message,
                exception.Detail);

            await context.Response.WriteAsync(JsonHelper.SerializeObject(result));
        }

        private async Task HandleUnexpectedException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var result = new ErrorResult(
                context.Response.StatusCode,
                "exception",
                exception.Message);

            await context.Response.WriteAsync(JsonHelper.SerializeObject(result));
        }
    }
}