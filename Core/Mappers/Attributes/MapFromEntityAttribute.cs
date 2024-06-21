using TripleSix.Core.Entities;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Cấu hình auto mapper từ type chỉ định.
    /// </summary>
    /// <typeparam name="TSource">Type nguồn.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapFromEntityAttribute<TSource> : Attribute
        where TSource : class, IEntity
    {
        /// <summary>
        /// Cấu hình auto mapper từ type chỉ định.
        /// </summary>
        /// <param name="ignoreProperties">Danh sách property đích sẽ không mapping.</param>
        public MapFromEntityAttribute(params string[] ignoreProperties)
        {
            IgnoreProperties = ignoreProperties;
        }

        /// <summary>
        /// Danh sách property đích sẽ không mapping.
        /// </summary>
        public string[]? IgnoreProperties { get; }
    }

    /// <summary>
    /// Cấu hình auto mapper từ type chỉ định.
    /// </summary>
    /// <typeparam name="TSource">Type nguồn.</typeparam>
    /// <typeparam name="TMappingAction">Custom mapping action.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapFromEntityAttribute<TSource, TMappingAction> : MapFromEntityAttribute<TSource>
        where TSource : class, IEntity
    {
        /// <summary>
        /// Cấu hình auto mapper từ type chỉ định.
        /// </summary>
        /// <param name="ignoreProperties">Danh sách property đích sẽ không mapping.</param>
        public MapFromEntityAttribute(params string[] ignoreProperties)
            : base(ignoreProperties)
        {
        }
    }
}
