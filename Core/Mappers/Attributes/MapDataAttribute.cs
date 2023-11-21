using AutoMapper;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Cấu hình auto mapper.
    /// </summary>
    /// <typeparam name="TSource">Type nguồn.</typeparam>
    /// <typeparam name="TDestination">Type đích.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapDataAttribute<TSource, TDestination> : Attribute
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Bỏ qua các property không có khai báo ở đối tượng nguồn.
        /// </summary>
        public bool IgnoreUndeclareProperty { get; set; }

        /// <summary>
        /// Danh sách property đích sẽ không mapping.
        /// </summary>
        public string[] IgnoreProperties { get; set; }
    }

    /// <summary>
    /// Cấu hình auto mapper.
    /// </summary>
    /// <typeparam name="TSource">Type nguồn.</typeparam>
    /// <typeparam name="TDestination">Type đích.</typeparam>
    /// <typeparam name="TMappingAction">Custom mapping action.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapDataAttribute<TSource, TDestination, TMappingAction> : MapDataAttribute<TSource, TDestination>
        where TSource : class
        where TDestination : class
        where TMappingAction : IMappingAction<TSource, TDestination>
    {
    }
}
