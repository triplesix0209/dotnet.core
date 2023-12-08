using System.ComponentModel;
using System.Reflection;
using Hangfire;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Hangfire
{
    /// <summary>
    /// Job option.
    /// </summary>
    public class JobOption
    {
        /// <summary>
        /// Job queue.
        /// </summary>
        public string? Queue { get; set; }

        /// <summary>
        /// Tên hiển thị của job.
        /// </summary>
        public string? DisplayName { get; set; }

        internal string GetMethodQueue(MethodInfo method)
        {
            if (Queue.IsNotNullOrEmpty()) return Queue;
            return method.GetCustomAttribute<QueueAttribute>()?.Queue ?? "default";
        }

        internal string GetMethodDisplayName(MethodInfo method)
        {
            var displayName = DisplayName;
            displayName ??= method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? method.Name;

            var assemblyName = method.DeclaringType?.Assembly.GetName().Name;
            if (assemblyName.IsNotNullOrEmpty()) displayName = $"[{assemblyName}] {displayName}";
            return displayName;
        }
    }
}
