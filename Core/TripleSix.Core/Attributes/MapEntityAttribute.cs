using TripleSix.Core.Entities;

namespace TripleSix.Core.Attributes
{
    /// <summary>
    /// Map với entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapEntityAttribute : Attribute
    {
        public MapEntityAttribute(Type entityType)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
                throw new Exception($"{entityType.Name} not implements {nameof(IEntity)}");

            EntityType = entityType;
        }

        /// <summary>
        /// Loại entity dùng để map.
        /// </summary>
        public Type EntityType { get; }
    }
}
