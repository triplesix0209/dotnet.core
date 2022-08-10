using TripleSix.Core.Entities;

namespace TripleSix.Core.AutoAdmin
{
    public static class AdminModel
    {
        public static Type? GetEntityType(Type? modelType)
        {
            Type? entityType = null;
            while (entityType == null && modelType != null)
            {
                if (!modelType.IsGenericType || modelType.GetGenericTypeDefinition() != typeof(AdminModel<>))
                {
                    modelType = modelType.BaseType;
                    continue;
                }

                entityType = modelType.GetGenericArguments()[0];
            }

            return entityType;
        }
    }

    public class AdminModel<TEntity>
        where TEntity : IStrongEntity
    {
    }
}
