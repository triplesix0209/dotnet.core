using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
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
        private static readonly MethodInfo _wrapWithResultMethod = typeof(ServiceInterceptor)
            .GetMethod(nameof(WrapWithResult), BindingFlags.NonPublic | BindingFlags.Static)!;

        private static readonly ConcurrentDictionary<Type, MethodInfo> _wrapMethodCache = new();

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            var parentActivity = Activity.Current;
            var activity = parentActivity?.Source.StartActivity(
                $"{invocation.TargetType.Name}.{invocation.Method.Name}");

            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                TagError(activity, e);
                activity?.Dispose();
                throw;
            }

            if (invocation.ReturnValue is not Task task)
            {
                activity?.Dispose();
                return;
            }

            // không block chờ task; wrap lại để giữ activity span trọn thời gian chạy async
            var returnType = invocation.Method.ReturnType;
            invocation.ReturnValue = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                ? _wrapMethodCache
                    .GetOrAdd(returnType.GetGenericArguments()[0], resultType => _wrapWithResultMethod.MakeGenericMethod(resultType))
                    .Invoke(null, [task, activity])
                : Wrap(task, activity);

            // Intercept trả về trước khi task xong; phải trả Activity.Current về parent để
            // các span kế tiếp của caller không bị neo vào activity chưa/đã dispose của interceptor
            if (activity != null)
                Activity.Current = parentActivity;
        }

        private static async Task Wrap(Task task, Activity? activity)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                TagError(activity, e);
                throw;
            }
            finally
            {
                activity?.Dispose();
            }
        }

        private static async Task<TResult> WrapWithResult<TResult>(Task<TResult> task, Activity? activity)
        {
            try
            {
                return await task;
            }
            catch (Exception e)
            {
                TagError(activity, e);
                throw;
            }
            finally
            {
                activity?.Dispose();
            }
        }

        private static void TagError(Activity? activity, Exception e)
        {
            if (activity == null) return;

            activity.AddTag("error", true);
            switch (e)
            {
                case BaseException baseException:
                    activity.AddTag("error_code", baseException.Code);
                    activity.AddTag("error_message", baseException.Message);
                    break;

                case AggregateException aggregate when aggregate.InnerExceptions.Count == 1 && aggregate.InnerException is BaseException innerException:
                    activity.AddTag("error_code", innerException.Code);
                    activity.AddTag("error_message", innerException.Message);
                    break;

                default:
                    activity.AddTag("error_message", e.FullMessage());
                    break;
            }
        }
    }
}
