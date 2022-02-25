using System;
using System.Linq;
using System.Reflection;

namespace TripleSix.Core.AutoAdmin
{
    public class AdminMetadata
    {
        public AdminMetadata()
        {
            Version = new Version(2, 0);

            var allControllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<AdminControllerAttribute>()?.Enable == true)
                .Where(t =>
                {
                    var info = t.GetCustomAttribute<AdminControllerAttribute>();
                    return info.EntityType is not null && info.AdminType is not null;
                });

            Controllers = allControllers
                .OrderBy(t => t.GetCustomAttribute<AdminControllerAttribute>()?.LoadOrder)
                .ThenBy(t => t.Name)
                .Select(t => new ControllerMetadata(t))
                .Where(x => x.Methods.Any())
                .ToArray();
        }

        public Version Version { get; set; }

        public ControllerMetadata[] Controllers { get; set; }
    }
}
