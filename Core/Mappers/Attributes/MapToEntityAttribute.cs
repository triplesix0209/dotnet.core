using TripleSix.Core.Entities;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Cấu hình auto mapper đến type chỉ định.
    /// </summary>
    /// <typeparam name="TDestination">Type đích.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapToEntityAttribute<TDestination> : Attribute
        where TDestination : class, IEntity
    {
        /// <summary>
        /// Cấu hình auto mapper đến type chỉ định.
        /// </summary>
        /// <param name="ignoreUndeclareProperty">Bỏ qua các property không có khai báo ở đối tượng nguồn.</param>
        public MapToEntityAttribute(bool ignoreUndeclareProperty = false)
        {
            IgnoreUndeclareProperty = ignoreUndeclareProperty;
        }

        /// <summary>
        /// Cấu hình auto mapper đến type chỉ định.
        /// </summary>
        /// <param name="ignoreProperties">Danh sách property đích sẽ không mapping.</param>
        public MapToEntityAttribute(params string[] ignoreProperties)
        {
            IgnoreUndeclareProperty = false;
            IgnoreProperties = ignoreProperties;
        }

        /// <summary>
        /// Bỏ qua các property không có khai báo ở đối tượng nguồn.
        /// </summary>
        public bool IgnoreUndeclareProperty { get; }

        /// <summary>
        /// Danh sách property đích sẽ không mapping.
        /// </summary>
        public string[]? IgnoreProperties { get; }
    }

    /// <summary>
    /// Cấu hình auto mapper đến type chỉ định.
    /// </summary>
    /// <typeparam name="TDestination">Type đích.</typeparam>
    /// <typeparam name="TMappingAction">Custom mapping action.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapToEntityAttribute<TDestination, TMappingAction> : MapToEntityAttribute<TDestination>
        where TDestination : class, IEntity
    {
        /// <summary>
        /// Cấu hình auto mapper đến type chỉ định.
        /// </summary>
        /// <param name="ignoreUndeclareProperty">Bỏ qua các property không có khai báo ở đối tượng nguồn.</param>
        public MapToEntityAttribute(bool ignoreUndeclareProperty = false)
            : base(ignoreUndeclareProperty)
        {
        }

        /// <summary>
        /// Cấu hình auto mapper đến type chỉ định.
        /// </summary>
        /// <param name="ignoreProperties">Danh sách property đích sẽ không mapping.</param>
        public MapToEntityAttribute(params string[] ignoreProperties)
            : base(ignoreProperties)
        {
        }
    }
}
