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
    }
}
