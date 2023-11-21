using System.Diagnostics;
using Castle.DynamicProxy;
using TripleSix.Core.Exceptions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.AutofacModules
{
    /// <summary>
    /// Interceptor cho các method của IService.
    /// </summary>
    internal class ServiceInterceptor : IInterceptor
    {
        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            using var activity = Activity.Current?.Source.StartActivity(
                $"{invocation.TargetType.Name}.{invocation.Method.Name}");
            {
                try
                {
                    invocation.Proceed();
                    var result = invocation.ReturnValue;
                    if (result is Task taskResult)
                        taskResult.Wait();
                }
                catch (BaseException e)
                {
                    activity?.AddTag("error", true);
                    activity?.AddTag("error_code", e.Code);
                    activity?.AddTag("error_message", e.Message);
                }
                catch (AggregateException e)
                {
                    activity?.AddTag("error", true);
                    if (e.InnerExceptions.Count == 1 && e.InnerException is BaseException exception)
                    {
                        activity?.AddTag("error_code", exception.Code);
                        activity?.AddTag("error_message", exception.Message);
                    }
                    else
                    {
                        activity?.AddTag("error_message", e.FullMessage());
                    }
                }
                catch (Exception e)
                {
                    activity?.AddTag("error", true);
                    activity?.AddTag("error_message", e.FullMessage());
                }
            }
        }
    }
}
