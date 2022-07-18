using System.Diagnostics;
using Castle.DynamicProxy;

namespace TripleSix.Core.AutofacModules
{
    /// <summary>
    /// Interceptor cho các method của IService.
    /// </summary>
    internal class ServiceInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            using var activity = Activity.Current?.Source.StartActivity(
                $"{invocation.TargetType.Name}.{invocation.Method.Name}");
            {
                invocation.Proceed();
                var result = invocation.ReturnValue;
                if (result is Task taskResult)
                    taskResult.Wait();
            }
        }
    }
}
