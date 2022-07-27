using TripleSix.Core.Entities;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Map entity với đối tượng.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapFromEntityAttribute : Attribute
    {
        /// <summary>
        /// Map entity với đối tượng chỉ định.
        /// </summary>
        /// <param name="entityType">Loại entity map.</param>
        /// <param name="ignoreUnmapedProperties">Bỏ qua các property không có trên DTO khi map.</param>
        public MapFromEntityAttribute(Type entityType)
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
