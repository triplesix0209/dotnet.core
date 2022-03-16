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
                .Where(t => t.GetCustomAttribute<AdminControllerAttribute>()?.AdminType is not null);

            Controllers = allControllers
                .OrderBy(t => t.GetCustomAttribute<AdminControllerAttribute>()?.LoadOrder)
                .ThenBy(t => t.Name)
                .Select(t => new ControllerMetadata(t))
                .Where(x => x.MethodData.Any())
                .ToArray();

            ControllerGroups = Controllers
                .Where(x => x.GroupData is not null)
                .Select(x => x.GroupData)
                .GroupBy(x => x.Code)
                .Select(x => x.First())
                .ToArray();

            Methods = Controllers
                .SelectMany(x => x.MethodData)
                .ToArray();
        }

        public Version Version { get; set; }

        public ControllerMetadata[] Controllers { get; set; }

        public GroupMetadata[] ControllerGroups { get; set; }

        public MethodMetadata[] Methods { get; set; }
    }
}
