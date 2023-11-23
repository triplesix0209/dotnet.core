using Elastic.Clients.Elasticsearch.QueryDsl;
using TripleSix.Core.Elastic;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Model tạo elastic query.
    /// </summary>
    /// <typeparam name="TDocument">Loại elastic document.</typeparam>
    public interface IElasticQueryableDto<TDocument>
        where TDocument : IElasticDocument
    {
        /// <summary>
        /// Chuyển hóa thành elastic query.
        /// </summary>
        /// <returns><see cref="Query"/>.</returns>
        Query? ToElasticQuery();
    }
}
