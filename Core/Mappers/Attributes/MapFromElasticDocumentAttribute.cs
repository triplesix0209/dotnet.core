using TripleSix.Core.Elastic;

namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Cấu hình auto mapper từ type chỉ định.
    /// </summary>
    /// <typeparam name="TSource">Type nguồn.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapFromElasticDocumentAttribute<TSource> : Attribute
        where TSource : class, IElasticDocument
    {
    }

    /// <summary>
    /// Cấu hình auto mapper từ type chỉ định.
    /// </summary>
    /// <typeparam name="TSource">Type nguồn.</typeparam>
    /// <typeparam name="TMappingAction">Custom mapping action.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MapFromElasticDocumentAttribute<TSource, TMappingAction> : MapFromElasticDocumentAttribute<TSource>
        where TSource : class, IElasticDocument
    {
    }
}
