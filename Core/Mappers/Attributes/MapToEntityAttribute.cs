using TripleSix.Core.Entities;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Map đối tượng với entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapToEntityAttribute : Attribute
    {
        /// <summary>
        /// Map đối tượng với entity chỉ định.
        /// </summary>
        /// <param name="entityType">Loại entity map.</param>
        public MapToEntityAttribute(Type entityType)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
                throw new Exception($"{entityType.Name} not implements {nameof(IEntity)}");

            EntityType = entityType;
        }

        /// <summary>
        /// Loại entity map.
        /// </summary>
        public Type EntityType { get; }
    }
}
