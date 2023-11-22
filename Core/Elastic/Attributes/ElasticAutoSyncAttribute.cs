namespace TripleSix.Core.Elastic
{
    /// <summary>
    /// Tự động đồng bộ với elastic.
    /// </summary>
    /// <typeparam name="TDocument">Elastic document chỉ định sẽ đồng bộ.</typeparam>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ElasticAutoSyncAttribute<TDocument> : Attribute
        where TDocument : class, IElasticDocument
    {
    }
}
