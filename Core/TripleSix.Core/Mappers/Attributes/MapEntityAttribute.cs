using TripleSix.Core.Entities;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Map với entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapEntityAttribute : Attribute
    {
        /// <summary>
        /// Map đối tượng với entity chỉ định.
        /// </summary>
        /// <param name="entityType">Loại entity map.</param>
        /// <param name="ignoreUnmapedProperties">Bỏ qua các property không có trên DTO khi map.</param>
        public MapEntityAttribute(Type entityType, bool ignoreUnmapedProperties = false)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
                throw new Exception($"{entityType.Name} not implements {nameof(IEntity)}");

            EntityType = entityType;
            IgnoreUnmapedProperties = ignoreUnmapedProperties;
        }

        /// <summary>
        /// Loại entity map.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Bỏ qua các property không có trên DTO khi map.
        /// </summary>
        public bool IgnoreUnmapedProperties { get; }
    }
}
