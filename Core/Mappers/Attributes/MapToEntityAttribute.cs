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
        /// <param name="unmapedProperties">Bỏ qua các property không có trên DTO khi map.</param>
        public MapToEntityAttribute(Type entityType, params string[] unmapedProperties)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
                throw new Exception($"{entityType.Name} not implements {nameof(IEntity)}");

            EntityType = entityType;
            UnmapedProperties = unmapedProperties;
        }

        /// <summary>
        /// Loại entity map.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Bỏ qua các property không có trên DTO khi map.
        /// </summary>
        public string[] UnmapedProperties { get; }
    }
}
