using System;
using System.Linq;
using System.Reflection;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.AutoAdmin
{
    public static class AdminHelper
    {
        public static Type GetEntityType(this Type adminType, string[] excludeAssemblyNames = null)
        {
            if (adminType is null || !adminType.IsAssignableTo<IAdminDto>())
                return null;

            string entityName;
            var adminModel = adminType.GetCustomAttribute<AdminModelAttribute>();
            if (adminModel is null)
            {
                entityName = adminType.Name.ToLower().EndsWith("admindto")
                        ? adminType.Name[0..^8]
                        : adminType.Name;
                entityName += "Entity";
            }
            else
            {
                entityName = adminModel.EntityName;
            }

            if (entityName.IsNullOrWhiteSpace()) return null;
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => excludeAssemblyNames == null || !excludeAssemblyNames.Contains(assembly.GetName().Name))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsAssignableTo<IEntity>())
                .Where(t => t.Name == entityName)
                .FirstOrDefault();
        }
    }
}
